
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


// Skonfigurowanie mapy
var osm = L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
});

var geoportal = L.tileLayer('https://mapy.geoportal.gov.pl/wss/service/PZGIK/ORTO/WMTS/StandardResolution?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER=StandardResolution&STYLE=default&TILEMATRIXSET=EPSG:3857&TILEMATRIX={z}&TILEROW={y}&TILECOL={x}', {
    attribution: 'Dane: <a href="https://www.gov.pl/web/gugik">GUGiK</a> / <a href="https://www.geoportal.gov.pl">geoportal.gov.pl</a>'
});

var map = L.map('map', {
    center: [52.12, 19.11],
    zoom: 6,
    layers: [osm]
});

var baseMaps = {
    "OpenStreetMap": osm,
    "Geoportal Orto": geoportal
};

L.control.scale({imperial: false, metric: true}).addTo(map);

// Dodaje menu w prawym górnym rogu do zmiany mapy
L.control.layers(baseMaps).addTo(map);

var marker;
var basemapSquare;

map.on('click', function(e) {
    var lat = e.latlng.lat;
    var lon = e.latlng.lng;
    var tileSize = 500;

    if (marker) {
        marker.setLatLng(e.latlng);
    } else {
        marker = L.marker(e.latlng).addTo(map);
    }

    drawBasemapSquare(lat, lon, tileSize);

    // Wysy³anie do C#
    var message = { lat: lat, lon: lon };
    if (window.chrome && window.chrome.webview) {
        window.chrome.webview.postMessage(message);
    }
});

function drawBasemapSquare(lat, lon, sizeInMeters) {
    // Jeœli kwadrat ju¿ istnieje, usuwamy go przed narysowaniem nowego
    if (basemapSquare) {
        map.removeLayer(basemapSquare);
    }

    var halfSize = sizeInMeters / 2;

    // Przeliczniki (uproszczone, ale bardzo dok³adne dla ma³ych obszarów)
    // 1 stopieñ szerokoœci to ok. 111 320 metrów
    var latOffset = halfSize / 111320;
    
    // D³ugoœæ stopnia d³ugoœci geograficznej zale¿y od szerokoœci (cosinus)
    var lonOffset = halfSize / (111320 * Math.cos(lat * Math.PI / 180));

    // Obliczamy naro¿niki kwadratu
    var southWest = L.latLng(lat - latOffset, lon - lonOffset);
    var northEast = L.latLng(lat + latOffset, lon + lonOffset);
    var bounds = L.latLngBounds(southWest, northEast);

    // Tworzymy kwadrat (Rectangle)
    basemapSquare = L.rectangle(bounds, {
        color: "#ff7800",      // Kolor linii
        weight: 2,             // Gruboœæ linii
        fillOpacity: 0.2,      // Przezroczystoœæ wype³nienia
        dashArray: '5, 5',     // Linia przerywana
        interactive: false     // Klikniêcia przechodz¹ "przez" kwadrat do mapy
    }).addTo(map);
}