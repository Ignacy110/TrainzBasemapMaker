
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
var map = L.map('map').setView([52.12, 19.11], 6);

L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
}).addTo(map);

var marker;

map.on('click', function(e) {
    if (marker) {
        marker.setLatLng(e.latlng);
    } else {
        marker = L.marker(e.latlng).addTo(map);
    }

    // Wysy³anie do C#
    var message = { lat: e.latlng.lat, lon: e.latlng.lng };
    if (window.chrome && window.chrome.webview) {
        window.chrome.webview.postMessage(message);
    }
});