var map = L.map('map').setView([52.12, 19.11], 6);

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; OpenStreetMap'
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