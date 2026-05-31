
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
//
//
// This script utilizes the Leaflet.js library (version 1.9.4)
// for handling map interactions, markers, and coordinate transmission.
// Leaflet.js is authored by Volodymyr Agafonkin and contributors
// and is licensed under the BSD 2-Clause License.
//
// Map tiles are provided by OpenStreetMap
// and are licensed under the Creative Commons Attribution-ShareAlike 2.0 (CC BY-SA).

// Initialize map tile layers
var osm = L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
});

var geoportal = L.tileLayer('https://mapy.geoportal.gov.pl/wss/service/PZGIK/ORTO/WMTS/StandardResolution?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER=StandardResolution&STYLE=default&TILEMATRIXSET=EPSG:3857&TILEMATRIX={z}&TILEROW={y}&TILECOL={x}', {
    attribution: 'Dane: <a href="https://www.gov.pl/web/gugik">GUGiK</a> / <a href="https://www.geoportal.gov.pl">geoportal.gov.pl</a>'
});

// Configure the main map instance centered over Poland by default
var map = L.map('map', {
    center: [52.12, 19.11],
    zoom: 6,
    layers: [osm]
});

var baseMaps = {
    "OpenStreetMap": osm,
    "Geoportal Orto": geoportal
};

// Add metric scale indicator to the bottom-left corner
L.control.scale({ imperial: false, metric: true }).addTo(map);

// Add the layer selection switcher menu to the top-right corner
L.control.layers(baseMaps).addTo(map);

var marker;
var basemapSquare;

// Global map click event listener
map.on('click', function(e) {
    var lat = e.latlng.lat;
    var lon = e.latlng.lng;
    var tileSize = 500; // Target basemap dimensions in meters (e.g., 500m x 500m)

    // Update existing marker position or create a new one on initial click
    if (marker) {
        marker.setLatLng(e.latlng);
    } else {
        marker = L.marker(e.latlng).addTo(map);
    }

    // Draw the preview boundary box representing the actual C# download footprint
    drawBasemapSquare(lat, lon, tileSize);

    // Transmit coordinates asynchronously back to the hosting WinForms C# WebView2 environment
    var message = { lat: lat, lon: lon };
    if (window.chrome && window.chrome.webview) {
        window.chrome.webview.postMessage(message);
    }
});

/**
 * Calculates and renders a precise geographic square boundary scaled in meters.
 * Handles Earth curvature distortion adjustments by accounting for latitude shrinkage.
 */
function drawBasemapSquare(lat, lon, sizeInMeters) {
    // Clear the previous bounding box layer if it already exists on the map viewport
    if (basemapSquare) {
        map.removeLayer(basemapSquare);
    }

    var halfSize = sizeInMeters / 2;

    // Conversion metrics (simplified projection, highly accurate for local bounding areas)
    // 1 degree of latitude remains roughly static at ~111,320 meters
    var latOffset = halfSize / 111320;
    
    // 1 degree of longitude shrinks toward the poles; scaled dynamically using the cosine of the current latitude
    var lonOffset = halfSize / (111320 * Math.cos(lat * Math.PI / 180));

    // Determine the exact geographic corners for the bounding box matrix
    var southWest = L.latLng(lat - latOffset, lon - lonOffset);
    var northEast = L.latLng(lat + latOffset, lon + lonOffset);
    var bounds = L.latLngBounds(southWest, northEast);

    // Create and style the interactive preview overlay rectangle
    basemapSquare = L.rectangle(bounds, {
        color: "#ff7800",       // Boundary stroke color
        weight: 2,              // Boundary stroke thickness in pixels
        fillOpacity: 0.2,       // Background fill transparency
        dashArray: '5, 5',      // Dotted/dashed stroke layout style
        interactive: false      // Disables pointer events so map click triggers pass right through the geometry
    }).addTo(map);
}