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
    layers: [osm],
    dragging: false,
    doubleClickZoom: false
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
var selectedTiles = new Map(); // key "i_j" => { i, j, x, y, status } (newly selected tiles for download)
var existingTiles = new Map(); // key "i_j" => { i, j, x, y, counter, status: "existing" } (tiles already loaded from folder)
var selectionMode = "click";   // "click" or "box"

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

// Get tile center in projected meters and Lat/Lon given grid index (i, j)
function getTileCenter(i, j) {
    if (!anchor) return null;

    if (currentEpsg === "EPSG:2180") {
        var x = anchor.x + i * tileSize;
        var y = anchor.y + j * tileSize;
        var ll = metersToLatLon(x, y);
        return { x: x, y: y, lat: ll.lat, lon: ll.lon };
    } else {
        // EPSG:3857: 500 real ground meters footprint based on anchor latitude
        var anchorLL = metersToLatLon(anchor.x, anchor.y);
        var cosLat = Math.cos(anchorLL.lat * Math.PI / 180.0);
        var stepSize = tileSize / Math.max(0.01, cosLat);

        var x = anchor.x + i * stepSize;
        var y = anchor.y + j * stepSize;
        var ll = metersToLatLon(x, y);
        return { x: x, y: y, lat: ll.lat, lon: ll.lon };
    }
}

// Compute corner LatLngs for a 500m tile
function getTileBoundsForIndex(i, j) {
    var center = getTileCenter(i, j);
    if (!center) return null;

    if (currentEpsg === "EPSG:2180") {
        var half = tileSize / 2.0;
        var sw = metersToLatLon(center.x - half, center.y - half);
        var nw = metersToLatLon(center.x - half, center.y + half);
        var ne = metersToLatLon(center.x + half, center.y + half);
        var se = metersToLatLon(center.x + half, center.y - half);
        return [
            [nw.lat, nw.lon],
            [ne.lat, ne.lon],
            [se.lat, se.lon],
            [sw.lat, sw.lon]
        ];
    } else {
        var anchorLL = metersToLatLon(anchor.x, anchor.y);
        var cosLat = Math.cos(anchorLL.lat * Math.PI / 180.0);
        var stepSize = tileSize / Math.max(0.01, cosLat);
        var halfSpan = stepSize / 2.0;

        var nw = metersToLatLon(center.x - halfSpan, center.y + halfSpan);
        var ne = metersToLatLon(center.x + halfSpan, center.y + halfSpan);
        var se = metersToLatLon(center.x + halfSpan, center.y - halfSpan);
        var sw = metersToLatLon(center.x - halfSpan, center.y - halfSpan);
        return [
            [nw.lat, nw.lon],
            [ne.lat, ne.lon],
            [se.lat, se.lon],
            [sw.lat, sw.lon]
        ];
    }
}

// Convert any clicked Lat/Lon to grid index (i, j)
function latLonToGridIndex(lat, lon) {
    if (!anchor) return null;

    if (currentEpsg === "EPSG:2180") {
        var m = latLonToMeters(lat, lon);
        var i = Math.round((m.x - anchor.x) / tileSize);
        var j = Math.round((m.y - anchor.y) / tileSize);
        return { i: i, j: j };
    } else {
        var anchorLL = metersToLatLon(anchor.x, anchor.y);
        var cosLat = Math.cos(anchorLL.lat * Math.PI / 180.0);
        var stepSize = tileSize / Math.max(0.01, cosLat);

        var m = latLonToMeters(lat, lon);
        var i = Math.round((m.x - anchor.x) / stepSize);
        var j = Math.round((m.y - anchor.y) / stepSize);
        return { i: i, j: j };
    }
}

// Update the visible local grid overlay
function updateGridOverlay() {
    gridLayerGroup.clearLayers();

    if (!anchor || map.getZoom() < 11) {
        return;
    }

    var bounds = map.getBounds();
    var nwIdx = latLonToGridIndex(bounds.getNorth(), bounds.getWest());
    var seIdx = latLonToGridIndex(bounds.getSouth(), bounds.getEast());

    if (!nwIdx || !seIdx) return;

    var minI = Math.min(nwIdx.i, seIdx.i) - 1;
    var maxI = Math.max(nwIdx.i, seIdx.i) + 1;
    var minJ = Math.min(nwIdx.j, seIdx.j) - 1;
    var maxJ = Math.max(nwIdx.j, seIdx.j) + 1;

    var countI = maxI - minI;
    var countJ = maxJ - minJ;
    if (countI * countJ > 2500) {
        return;
    }

    for (var i = minI; i <= maxI; i++) {
        for (var j = minJ; j <= maxJ; j++) {
            var key = i + "_" + j;
            if (selectedTiles.has(key) || existingTiles.has(key)) continue;

            var polyCorners = getTileBoundsForIndex(i, j);
            if (!polyCorners) continue;

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

// Get sorted list of selected new tiles (top-to-bottom, left-to-right)
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

// Redraw all tiles with badges
function renderSelectedTiles(shouldNotify) {
    if (shouldNotify === undefined) shouldNotify = true;
    selectedLayerGroup.clearLayers();

    // 1. Render existing tiles loaded from folder (Green)
    existingTiles.forEach(function(tile, key) {
        if (selectedTiles.has(key)) return; // If user explicitly selected it, render as new/modified

        var polyCorners = getTileBoundsForIndex(tile.i, tile.j);
        if (!polyCorners) return;

        var poly = L.polygon(polyCorners, {
            color: "#1e7e34",
            weight: 2,
            fillColor: "#28a745",
            fillOpacity: 0.35,
            interactive: false
        });
        selectedLayerGroup.addLayer(poly);

        var center = getTileCenter(tile.i, tile.j);
        var labelHtml = '<div class="tile-number-label done">#' + tile.counter + '</div>';

        var numIcon = L.divIcon({
            className: '',
            html: labelHtml,
            iconSize: [24, 24],
            iconAnchor: [12, 12]
        });

        var marker = L.marker([center.lat, center.lon], {
            icon: numIcon,
            interactive: false
        });
        selectedLayerGroup.addLayer(marker);
    });

    // 2. Render newly selected tiles for download (Blue/Orange/Green)
    var sorted = getSortedTiles();

    sorted.forEach(function(tile, index) {
        var orderNum = index + 1;
        var polyCorners = getTileBoundsForIndex(tile.i, tile.j);
        if (!polyCorners) return;

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

        var center = getTileCenter(tile.i, tile.j);

        var badgeClass = "tile-number-label";
        if (tile.status === "downloading") badgeClass += " downloading";
        else if (tile.status === "done") badgeClass += " done";
        else if (tile.status === "error") badgeClass += " error";

        var labelHtml = '<div class="' + badgeClass + '">' + orderNum + '</div>';

        var numIcon = L.divIcon({
            className: '',
            html: labelHtml,
            iconSize: [24, 24],
            iconAnchor: [12, 12]
        });

        var marker = L.marker([center.lat, center.lon], {
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
    var sortedNew = getSortedTiles();
    var existingList = Array.from(existingTiles.values());

    var payload = {
        type: "selection_changed",
        epsg: currentEpsg,
        anchor: anchor,
        count: sortedNew.length,
        existingCount: existingList.length,
        tiles: sortedNew.map(function(t, idx) {
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
        var center = getTileCenter(i, j);
        selectedTiles.set(key, {
            i: i,
            j: j,
            x: center.x,
            y: center.y,
            status: "normal"
        });
    }
    renderSelectedTiles(true);
    updateGridOverlay();
}

// Add a tile if not already present
function addTile(i, j) {
    if (!anchor) return;
    var key = i + "_" + j;
    if (!selectedTiles.has(key)) {
        var center = getTileCenter(i, j);
        selectedTiles.set(key, {
            i: i,
            j: j,
            x: center.x,
            y: center.y,
            status: "normal"
        });
    }
}

map.on('moveend zoomend', function() {
    updateGridOverlay();
});

// Selection modes
function setSelectionMode(mode) {
    selectionMode = mode;
}

var mapContainer = document.getElementById("map");
var selectionBox = document.getElementById("selectionBox");

// Panning state (Right click or Middle click)
var isPanning = false;
var panStartPoint = null;

// Brush & Single-click state (Left click)
var isPainting = false;
var paintMoved = false;
var initialTileInfo = null; // { i, j, wasSelected }

// Box selection state (Left click in Box mode)
var isSelecting = false;
var boxStartPoint = null;

// Prevent context menu so right click can be used for panning
mapContainer.addEventListener("contextmenu", function(e) {
    e.preventDefault();
});

mapContainer.addEventListener("mousedown", function(e) {
    // 1. Right click (2) or Middle click (1) -> Pan Map
    if (e.button === 2 || e.button === 1) {
        isPanning = true;
        panStartPoint = { x: e.clientX, y: e.clientY };
        mapContainer.style.cursor = "grabbing";
        e.preventDefault();
        return;
    }

    // 2. Left click (0)
    if (e.button === 0) {
        var containerRect = mapContainer.getBoundingClientRect();
        var latlng = map.containerPointToLatLng([
            e.clientX - containerRect.left,
            e.clientY - containerRect.top
        ]);

        if (selectionMode === "box") {
            // Box selection start
            isSelecting = true;
            boxStartPoint = { x: e.clientX, y: e.clientY };

            selectionBox.style.left = boxStartPoint.x + "px";
            selectionBox.style.top = boxStartPoint.y + "px";
            selectionBox.style.width = "0px";
            selectionBox.style.height = "0px";
            selectionBox.style.display = "block";
            e.preventDefault();
        } else {
            // "Pędzel / klikanie" mode
            isPainting = true;
            paintMoved = false;

            if (!anchor) {
                var clicked = latLonToMeters(latlng.lat, latlng.lng);
                anchor = {
                    x: Math.round(clicked.x),
                    y: Math.round(clicked.y)
                };

                var anchorLL = metersToLatLon(anchor.x, anchor.y);
                anchorMarker = L.circleMarker([anchorLL.lat, anchorLL.lon], {
                    radius: 5,
                    color: '#ff7800',
                    fillColor: '#ff7800',
                    fillOpacity: 1
                }).addTo(map);

                addTile(0, 0);
                initialTileInfo = { i: 0, j: 0, wasSelected: false };
                renderSelectedTiles(false);
                updateGridOverlay();
            } else {
                var idx = latLonToGridIndex(latlng.lat, latlng.lng);
                if (idx) {
                    var key = idx.i + "_" + idx.j;
                    var wasSelected = selectedTiles.has(key);
                    initialTileInfo = { i: idx.i, j: idx.j, wasSelected: wasSelected };

                    if (!wasSelected) {
                        addTile(idx.i, idx.j);
                        renderSelectedTiles(false);
                    }
                }
            }
            e.preventDefault();
        }
    }
});

window.addEventListener("mousemove", function(e) {
    // 1. Panning
    if (isPanning && panStartPoint) {
        var dx = e.clientX - panStartPoint.x;
        var dy = e.clientY - panStartPoint.y;
        map.panBy([-dx, -dy], { animate: false });
        panStartPoint = { x: e.clientX, y: e.clientY };
        return;
    }

    // 2. Brush painting (Left button held down)
    if (isPainting && selectionMode === "click" && anchor) {
        var containerRect = mapContainer.getBoundingClientRect();
        var latlng = map.containerPointToLatLng([
            e.clientX - containerRect.left,
            e.clientY - containerRect.top
        ]);

        var idx = latLonToGridIndex(latlng.lat, latlng.lng);
        if (idx) {
            var key = idx.i + "_" + idx.j;
            if (initialTileInfo && (idx.i !== initialTileInfo.i || idx.j !== initialTileInfo.j)) {
                paintMoved = true;
            }

            if (!selectedTiles.has(key)) {
                addTile(idx.i, idx.j);
                renderSelectedTiles(false);
            }
        }
        return;
    }

    // 3. Box rubber-band resizing
    if (isSelecting && boxStartPoint) {
        var currentX = e.clientX;
        var currentY = e.clientY;

        var left = Math.min(boxStartPoint.x, currentX);
        var top = Math.min(boxStartPoint.y, currentY);
        var width = Math.abs(currentX - boxStartPoint.x);
        var height = Math.abs(currentY - boxStartPoint.y);

        selectionBox.style.left = left + "px";
        selectionBox.style.top = top + "px";
        selectionBox.style.width = width + "px";
        selectionBox.style.height = height + "px";
    }
});

window.addEventListener("mouseup", function(e) {
    // 1. Panning stop
    if (isPanning) {
        isPanning = false;
        panStartPoint = null;
        mapContainer.style.cursor = "";
    }

    // 2. Brush painting stop
    if (isPainting) {
        isPainting = false;

        // If it was a single click without moving to other tiles and the tile was already selected -> deselect it
        if (!paintMoved && initialTileInfo && initialTileInfo.wasSelected) {
            var key = initialTileInfo.i + "_" + initialTileInfo.j;
            selectedTiles.delete(key);
        }

        initialTileInfo = null;
        renderSelectedTiles(true);
        updateGridOverlay();
    }

    // 3. Box selection commit
    if (isSelecting && boxStartPoint) {
        isSelecting = false;
        selectionBox.style.display = "none";

        var currentX = e.clientX;
        var currentY = e.clientY;

        var width = Math.abs(currentX - boxStartPoint.x);
        var height = Math.abs(currentY - boxStartPoint.y);

        if (width >= 5 || height >= 5) {
            var containerRect = mapContainer.getBoundingClientRect();
            var p1 = map.containerPointToLatLng([
                Math.min(boxStartPoint.x, currentX) - containerRect.left,
                Math.min(boxStartPoint.y, currentY) - containerRect.top
            ]);
            var p2 = map.containerPointToLatLng([
                Math.max(boxStartPoint.x, currentX) - containerRect.left,
                Math.max(boxStartPoint.y, currentY) - containerRect.top
            ]);

            if (!anchor) {
                var centerLat = (p1.lat + p2.lat) / 2.0;
                var centerLon = (p1.lng + p2.lng) / 2.0;
                var m = latLonToMeters(centerLat, centerLon);
                anchor = {
                    x: Math.round(m.x),
                    y: Math.round(m.y)
                };
                var anchorLL = metersToLatLon(anchor.x, anchor.y);
                anchorMarker = L.circleMarker([anchorLL.lat, anchorLL.lon], {
                    radius: 5,
                    color: '#ff7800',
                    fillColor: '#ff7800',
                    fillOpacity: 1
                }).addTo(map);
            }

            var idx1 = latLonToGridIndex(p1.lat, p1.lng);
            var idx2 = latLonToGridIndex(p2.lat, p2.lng);

            if (idx1 && idx2) {
                var minI = Math.min(idx1.i, idx2.i);
                var maxI = Math.max(idx1.i, idx2.i);
                var minJ = Math.min(idx1.j, idx2.j);
                var maxJ = Math.max(idx1.j, idx2.j);

                for (var i = minI; i <= maxI; i++) {
                    for (var j = minJ; j <= maxJ; j++) {
                        addTile(i, j);
                    }
                }
            }

            renderSelectedTiles(true);
            updateGridOverlay();
        }
        boxStartPoint = null;
    }
});

// Control API methods accessible from C#
function setCoordinateSystem(epsg) {
    if (currentEpsg === epsg) return;

    if (anchor) {
        var anchorLL = metersToLatLon(anchor.x, anchor.y);
        currentEpsg = epsg;
        var newM = latLonToMeters(anchorLL.lat, anchorLL.lon);
        anchor = {
            x: Math.round(newM.x),
            y: Math.round(newM.y)
        };

        selectedTiles.forEach(function(tile, key) {
            var center = getTileCenter(tile.i, tile.j);
            if (center) {
                tile.x = center.x;
                tile.y = center.y;
            }
        });

        existingTiles.forEach(function(tile, key) {
            var center = getTileCenter(tile.i, tile.j);
            if (center) {
                tile.x = center.x;
                tile.y = center.y;
            }
        });

        renderSelectedTiles(true);
        updateGridOverlay();
    } else {
        currentEpsg = epsg;
        resetGridOrigin();
    }
}

function clearAllTiles() {
    selectedTiles.clear();
    renderSelectedTiles(true);
    updateGridOverlay();
}

function resetGridOrigin() {
    selectedTiles.clear();
    existingTiles.clear();
    anchor = null;
    if (anchorMarker) {
        map.removeLayer(anchorMarker);
        anchorMarker = null;
    }
    renderSelectedTiles(true);
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

// Load existing tiles from folder and align grid
function loadExistingFolderTiles(data) {
    if (!data) return;

    if (data.epsg && data.epsg !== currentEpsg) {
        currentEpsg = data.epsg;
    }

    selectedTiles.clear();
    existingTiles.clear();

    if (data.anchor) {
        anchor = { x: Math.round(data.anchor.x), y: Math.round(data.anchor.y) };
    } else if (data.tiles && data.tiles.length > 0) {
        anchor = { x: Math.round(data.tiles[0].x), y: Math.round(data.tiles[0].y) };
    }

    if (!anchor) return;

    if (anchorMarker) {
        map.removeLayer(anchorMarker);
    }
    var anchorLL = metersToLatLon(anchor.x, anchor.y);
    anchorMarker = L.circleMarker([anchorLL.lat, anchorLL.lon], {
        radius: 6,
        color: '#ff7800',
        fillColor: '#ff7800',
        fillOpacity: 1
    }).addTo(map);

    var allBounds = [];

    if (data.tiles && data.tiles.length > 0) {
        data.tiles.forEach(function(tile) {
            var tileLL = metersToLatLon(tile.x, tile.y);
            var idx = latLonToGridIndex(tileLL.lat, tileLL.lon);
            if (!idx) return;

            var key = idx.i + "_" + idx.j;
            var center = getTileCenter(idx.i, idx.j);

            existingTiles.set(key, {
                i: idx.i,
                j: idx.j,
                x: center.x,
                y: center.y,
                counter: tile.counter,
                status: "existing"
            });

            var b = getTileBoundsForIndex(idx.i, idx.j);
            if (b) {
                b.forEach(function(pt) { allBounds.push(pt); });
            }
        });
    }

    if (allBounds.length > 0) {
        map.fitBounds(allBounds, { padding: [50, 50], maxZoom: 16 });
    } else {
        map.panTo([anchorLL.lat, anchorLL.lon]);
    }

    renderSelectedTiles(true);
    updateGridOverlay();
}
