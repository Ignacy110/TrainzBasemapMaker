// Trainz Basemap Maker
// https://github.com/Ignacy110/TrainzBasemapMaker
//
// Copyright (C) 2026 Ignacy110 (http://github.com/Ignacy110)
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, see (http://www.gnu.org/licenses/).

// Define Proj4 coordinate systems
proj4.defs("EPSG:2180", "+proj=tmerc +lat_0=0 +lon_0=19 +k=0.9993 +x_0=500000 +y_0=-5300000 +ellps=GRS80 +towgs84=0,0,0,0,0,0,0 +units=m +no_defs");
proj4.defs("EPSG:3857", "+proj=merc +a=6378137 +b=6378137 +lat_ts=0 +lon_0=0 +x_0=0 +y_0=0 +k=1 +units=m +nadgrids=@null +wktext +no_defs");

// Initialize map tile layers
var osm = L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
});

var geoportal = L.tileLayer('https://mapy.geoportal.gov.pl/wss/service/PZGIK/ORTO/WMTS/StandardResolution?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER=StandardResolution&STYLE=default&TILEMATRIXSET=EPSG:3857&TILEMATRIX={z}&TILEROW={y}&TILECOL={x}', {
    attribution: 'Dane: <a href="https://www.gov.pl/web/gugik">GUGiK</a> / <a href="https://www.geoportal.gov.pl">geoportal.gov.pl</a>'
});

var topo = L.tileLayer.wms('https://mapy.geoportal.gov.pl/wss/service/img/guest/TOPO/MapServer/WMSServer', {
    layers: 'Raster',
    format: 'image/jpeg',
    transparent: false,
    version: '1.1.1',
    maxZoom: 19,
    attribution: 'Dane: <a href="https://www.gov.pl/web/gugik">GUGiK</a> / <a href="https://www.geoportal.gov.pl">geoportal.gov.pl</a>'
});

var openRailwayMapOverlay = L.tileLayer('https://{s}.tiles.openrailwaymap.org/standard/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: 'Dane: &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> | Styl: &copy; <a href="https://www.openrailwaymap.org">OpenRailwayMap</a>'
});

var openRailwayMapGroup = L.layerGroup([
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', { maxZoom: 19 }),
    openRailwayMapOverlay
]);

// Initialize Leaflet map
var map = L.map('map', {
    center: [52.12, 19.11],
    zoom: 7,
    layers: [osm]
});

var baseMaps = {
    "OpenStreetMap": osm,
    "OpenRailwayMap": openRailwayMapGroup,
    "Geoportal Orto": geoportal,
    "Mapa topograficzna": topo
};

L.control.scale({ imperial: false, metric: true }).addTo(map);
L.control.layers(baseMaps).addTo(map);

// State variables
var currentEpsg = "EPSG:2180";
var tileSize = 500; // 500 meters
var anchor = null;  // { x: number, y: number } in projected meters
var selectedTiles = new Map(); // key "i_j" => { i, j, x, y, status, polygon, marker }
var selectionMode = "click"; // "click" or "box"

// Layer groups
var gridLayerGroup = L.layerGroup().addTo(map);
var selectedLayerGroup = L.layerGroup().addTo(map);
var anchorMarker = null;

// Coordinate transformations
function latLonToMeters(lat, lon) {
    var p = proj4("EPSG:4326", currentEpsg, [lon, lat]);
    return { x: p[0], y: p[1] };
}

function metersToLatLon(x, y) {
    var p = proj4(currentEpsg, "EPSG:4326", [x, y]);
    return { lat: p[1], lon: p[0] };
}

// Compute corner LatLngs for a 500m tile centered at (x, y)
function getTileBounds(xCenter, yCenter) {
    var half = tileSize / 2.0;
    var sw = metersToLatLon(xCenter - half, yCenter - half);
    var nw = metersToLatLon(xCenter - half, yCenter + half);
    var ne = metersToLatLon(xCenter + half, yCenter + half);
    var se = metersToLatLon(xCenter + half, yCenter - half);
    return [
        [nw.lat, nw.lon],
        [ne.lat, ne.lon],
        [se.lat, se.lon],
        [sw.lat, sw.lon]
    ];
}

// Update the visible local grid
function updateGridOverlay() {
    gridLayerGroup.clearLayers();

    if (!anchor || map.getZoom() < 11) {
        return;
    }

    var bounds = map.getBounds();
    var nw = latLonToMeters(bounds.getNorth(), bounds.getWest());
    var ne = latLonToMeters(bounds.getNorth(), bounds.getEast());
    var sw = latLonToMeters(bounds.getSouth(), bounds.getWest());
    var se = latLonToMeters(bounds.getSouth(), bounds.getEast());

    var minX = Math.min(nw.x, sw.x, ne.x, se.x);
    var maxX = Math.max(nw.x, sw.x, ne.x, se.x);
    var minY = Math.min(nw.y, sw.y, ne.y, se.y);
    var maxY = Math.max(nw.y, sw.y, ne.y, se.y);

    var minI = Math.floor((minX - anchor.x + tileSize / 2.0) / tileSize) - 1;
    var maxI = Math.floor((maxX - anchor.x + tileSize / 2.0) / tileSize) + 1;
    var minJ = Math.floor((minY - anchor.y + tileSize / 2.0) / tileSize) - 1;
    var maxJ = Math.floor((maxY - anchor.y + tileSize / 2.0) / tileSize) + 1;

    // Limit grid lines to prevent browser freeze when zoomed out
    var countI = maxI - minI;
    var countJ = maxJ - minJ;
    if (countI * countJ > 2500) {
        return;
    }

    for (var i = minI; i <= maxI; i++) {
        for (var j = minJ; j <= maxJ; j++) {
            var key = i + "_" + j;
            if (selectedTiles.has(key)) continue; // Selected tiles are rendered separately

            var cx = anchor.x + i * tileSize;
            var cy = anchor.y + j * tileSize;
            var polyCorners = getTileBounds(cx, cy);

            var poly = L.polygon(polyCorners, {
                color: "#777777",
                weight: 1,
                dashArray: "3, 3",
                fillColor: "#ffffff",
                fillOpacity: 0.02,
                interactive: false
            });
            gridLayerGroup.addLayer(poly);
        }
    }
}

// Get sorted list of selected tiles (top-to-bottom, left-to-right)
function getSortedTiles() {
    var list = Array.from(selectedTiles.values());
    list.sort(function(a, b) {
        if (b.j !== a.j) {
            return b.j - a.j; // Top rows first (higher Y / higher j)
        }
        return a.i - b.i; // Left to right (lower X / lower i)
    });
    return list;
}

// Redraw all selected tiles with their sequence numbers
function renderSelectedTiles(shouldNotify) {
    if (shouldNotify === undefined) shouldNotify = true;
    selectedLayerGroup.clearLayers();

    var sorted = getSortedTiles();
    document.getElementById("badgeCount").innerText = "Kafle: " + sorted.length;

    sorted.forEach(function(tile, index) {
        var orderNum = index + 1;
        var polyCorners = getTileBounds(tile.x, tile.y);

        var fillColor = "#0078d4";
        var borderColor = "#004578";

        if (tile.status === "downloading") {
            fillColor = "#ff9900";
            borderColor = "#cc7a00";
        } else if (tile.status === "done") {
            fillColor = "#28a745";
            borderColor = "#1e7e34";
        } else if (tile.status === "error") {
            fillColor = "#dc3545";
            borderColor = "#bd2130";
        }

        var isAnchorTile = (tile.i === 0 && tile.j === 0);

        var poly = L.polygon(polyCorners, {
            color: borderColor,
            weight: isAnchorTile ? 3 : 2,
            fillColor: fillColor,
            fillOpacity: 0.35,
            interactive: false
        });
        selectedLayerGroup.addLayer(poly);

        var centerLatLon = metersToLatLon(tile.x, tile.y);

        var badgeClass = "tile-number-label";
        if (tile.status === "downloading") badgeClass += " downloading";
        else if (tile.status === "done") badgeClass += " done";
        else if (tile.status === "error") badgeClass += " error";

        var labelHtml = '<div class="' + badgeClass + '">' + orderNum + (isAnchorTile ? ' (Baza)' : '') + '</div>';

        var numIcon = L.divIcon({
            className: '',
            html: labelHtml,
            iconSize: [40, 20],
            iconAnchor: [20, 10]
        });

        var marker = L.marker([centerLatLon.lat, centerLatLon.lon], {
            icon: numIcon,
            interactive: false
        });
        selectedLayerGroup.addLayer(marker);
    });

    if (shouldNotify) {
        notifySelectionChanged();
    }
}

// Transmit current state to C# WinForms WebView2 host
function notifySelectionChanged() {
    var sorted = getSortedTiles();
    var payload = {
        type: "selection_changed",
        epsg: currentEpsg,
        anchor: anchor,
        count: sorted.length,
        tiles: sorted.map(function(t, idx) {
            return {
                order: idx + 1,
                i: t.i,
                j: t.j,
                x: Math.round(t.x),
                y: Math.round(t.y)
            };
        })
    };

    if (window.chrome && window.chrome.webview) {
        window.chrome.webview.postMessage(payload);
    }
}

// Toggle a single tile at index (i, j)
function toggleTile(i, j) {
    if (!anchor) return;
    var key = i + "_" + j;
    if (selectedTiles.has(key)) {
        selectedTiles.delete(key);
    } else {
        var cx = anchor.x + i * tileSize;
        var cy = anchor.y + j * tileSize;
        selectedTiles.set(key, {
            i: i,
            j: j,
            x: cx,
            y: cy,
            status: "normal"
        });
    }
    renderSelectedTiles();
    updateGridOverlay();
}

// Add a tile if not already present
function addTile(i, j) {
    if (!anchor) return;
    var key = i + "_" + j;
    if (!selectedTiles.has(key)) {
        var cx = anchor.x + i * tileSize;
        var cy = anchor.y + j * tileSize;
        selectedTiles.set(key, {
            i: i,
            j: j,
            x: cx,
            y: cy,
            status: "normal"
        });
    }
}

// Map click event
map.on('click', function(e) {
    if (selectionMode === "box") return; // Handled by box selector

    var clicked = latLonToMeters(e.latlng.lat, e.latlng.lng);

    if (!anchor) {
        // First click initializes the anchor point
        anchor = {
            x: Math.round(clicked.x),
            y: Math.round(clicked.y)
        };

        // Create anchor marker
        var anchorLL = metersToLatLon(anchor.x, anchor.y);
        anchorMarker = L.circleMarker([anchorLL.lat, anchorLL.lon], {
            radius: 5,
            color: '#ff7800',
            fillColor: '#ff7800',
            fillOpacity: 1
        }).addTo(map);

        // Select the origin tile (0, 0)
        toggleTile(0, 0);
    } else {
        var i = Math.round((clicked.x - anchor.x) / tileSize);
        var j = Math.round((clicked.y - anchor.y) / tileSize);
        toggleTile(i, j);
    }
});

map.on('moveend zoomend', function() {
    updateGridOverlay();
});

// Selection modes
function setSelectionMode(mode) {
    selectionMode = mode;
    document.getElementById("btnModeClick").className = (mode === "click") ? "active" : "";
    document.getElementById("btnModeBox").className = (mode === "box") ? "active" : "";

    if (mode === "box") {
        map.dragging.disable();
    } else {
        map.dragging.enable();
    }
}

// Box Selection logic (Rubber-band)
var isSelecting = false;
var startPoint = null;
var selectionBox = document.getElementById("selectionBox");

var mapContainer = document.getElementById("map");

mapContainer.addEventListener("mousedown", function(e) {
    if (selectionMode !== "box" && !e.shiftKey) return;
    if (e.button !== 0) return;

    isSelecting = true;
    startPoint = { x: e.clientX, y: e.clientY };

    selectionBox.style.left = startPoint.x + "px";
    selectionBox.style.top = startPoint.y + "px";
    selectionBox.style.width = "0px";
    selectionBox.style.height = "0px";
    selectionBox.style.display = "block";
});

window.addEventListener("mousemove", function(e) {
    if (!isSelecting || !startPoint) return;

    var currentX = e.clientX;
    var currentY = e.clientY;

    var left = Math.min(startPoint.x, currentX);
    var top = Math.min(startPoint.y, currentY);
    var width = Math.abs(currentX - startPoint.x);
    var height = Math.abs(currentY - startPoint.y);

    selectionBox.style.left = left + "px";
    selectionBox.style.top = top + "px";
    selectionBox.style.width = width + "px";
    selectionBox.style.height = height + "px";
});

window.addEventListener("mouseup", function(e) {
    if (!isSelecting || !startPoint) return;
    isSelecting = false;
    selectionBox.style.display = "none";

    var currentX = e.clientX;
    var currentY = e.clientY;

    var width = Math.abs(currentX - startPoint.x);
    var height = Math.abs(currentY - startPoint.y);

    if (width < 5 && height < 5) return; // Ignore accidental tiny clicks

    var containerRect = mapContainer.getBoundingClientRect();
    var p1 = map.containerPointToLatLng([
        Math.min(startPoint.x, currentX) - containerRect.left,
        Math.min(startPoint.y, currentY) - containerRect.top
    ]);
    var p2 = map.containerPointToLatLng([
        Math.max(startPoint.x, currentX) - containerRect.left,
        Math.max(startPoint.y, currentY) - containerRect.top
    ]);

    var m1 = latLonToMeters(p1.lat, p1.lng);
    var m2 = latLonToMeters(p2.lat, p2.lng);

    var minX = Math.min(m1.x, m2.x);
    var maxX = Math.max(m1.x, m2.x);
    var minY = Math.min(m1.y, m2.y);
    var maxY = Math.max(m1.y, m2.y);

    if (!anchor) {
        var centerX = (minX + maxX) / 2.0;
        var centerY = (minY + maxY) / 2.0;
        anchor = {
            x: Math.round(centerX),
            y: Math.round(centerY)
        };
        var anchorLL = metersToLatLon(anchor.x, anchor.y);
        anchorMarker = L.circleMarker([anchorLL.lat, anchorLL.lon], {
            radius: 5,
            color: '#ff7800',
            fillColor: '#ff7800',
            fillOpacity: 1
        }).addTo(map);
    }

    var minI = Math.round((minX - anchor.x) / tileSize);
    var maxI = Math.round((maxX - anchor.x) / tileSize);
    var minJ = Math.round((minY - anchor.y) / tileSize);
    var maxJ = Math.round((maxY - anchor.y) / tileSize);

    for (var i = Math.min(minI, maxI); i <= Math.max(minI, maxI); i++) {
        for (var j = Math.min(minJ, maxJ); j <= Math.max(minJ, maxJ); j++) {
            addTile(i, j);
        }
    }

    renderSelectedTiles();
    updateGridOverlay();
});

// Control API methods accessible from C#
function setCoordinateSystem(epsg) {
    if (currentEpsg === epsg) return;
    currentEpsg = epsg;
    resetGridOrigin();
}

function setAnchorPoint(x, y) {
    anchor = { x: Math.round(x), y: Math.round(y) };
    selectedTiles.clear();

    if (anchorMarker) {
        map.removeLayer(anchorMarker);
    }

    var anchorLL = metersToLatLon(anchor.x, anchor.y);
    anchorMarker = L.circleMarker([anchorLL.lat, anchorLL.lon], {
        radius: 5,
        color: '#ff7800',
        fillColor: '#ff7800',
        fillOpacity: 1
    }).addTo(map);

    map.panTo([anchorLL.lat, anchorLL.lon]);
    addTile(0, 0);
    renderSelectedTiles();
    updateGridOverlay();
}

function clearAllTiles() {
    selectedTiles.clear();
    renderSelectedTiles();
    updateGridOverlay();
}

function resetGridOrigin() {
    selectedTiles.clear();
    anchor = null;
    if (anchorMarker) {
        map.removeLayer(anchorMarker);
        anchorMarker = null;
    }
    renderSelectedTiles();
    updateGridOverlay();
}

function selectCurrentViewport() {
    if (map.getZoom() < 12) {
        alert("Przybliż mapę (zoom >= 12), aby zaznaczyć widok.");
        return;
    }

    var bounds = map.getBounds();
    var nw = latLonToMeters(bounds.getNorth(), bounds.getWest());
    var se = latLonToMeters(bounds.getSouth(), bounds.getEast());

    var minX = Math.min(nw.x, se.x);
    var maxX = Math.max(nw.x, se.x);
    var minY = Math.min(nw.y, se.y);
    var maxY = Math.max(nw.y, se.y);

    if (!anchor) {
        var centerX = (minX + maxX) / 2.0;
        var centerY = (minY + maxY) / 2.0;
        anchor = {
            x: Math.round(centerX),
            y: Math.round(centerY)
        };
        var anchorLL = metersToLatLon(anchor.x, anchor.y);
        anchorMarker = L.circleMarker([anchorLL.lat, anchorLL.lon], {
            radius: 5,
            color: '#ff7800',
            fillColor: '#ff7800',
            fillOpacity: 1
        }).addTo(map);
    }

    var minI = Math.round((minX - anchor.x) / tileSize);
    var maxI = Math.round((maxX - anchor.x) / tileSize);
    var minJ = Math.round((minY - anchor.y) / tileSize);
    var maxJ = Math.round((maxY - anchor.y) / tileSize);

    for (var i = Math.min(minI, maxI); i <= Math.max(minI, maxI); i++) {
        for (var j = Math.min(minJ, maxJ); j <= Math.max(minJ, maxJ); j++) {
            addTile(i, j);
        }
    }

    renderSelectedTiles();
    updateGridOverlay();
}

// Highlight progress of tile downloading
function highlightTile(order, status) {
    var sorted = getSortedTiles();
    var target = sorted.find(function(t, idx) { return (idx + 1) === order; });
    if (target) {
        target.status = status;
        renderSelectedTiles(false);
    }
}
