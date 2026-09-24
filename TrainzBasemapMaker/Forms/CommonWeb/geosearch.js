// Trainz Basemap Maker - Leaflet GeoSearch Control
// Reusable city/town search control powered by Nominatim (OpenStreetMap)

function initLeafletGeoSearch(mapInstance, options) {
    options = options || {};

    var GeoSearchControl = L.Control.extend({
        options: {
            position: options.position || 'topleft',
            placeholder: options.placeholder || 'Szukaj miejscowości...'
        },

        onAdd: function() {
            var container = L.DomUtil.create('div', 'leaflet-control-geosearch leaflet-bar');

            L.DomEvent.disableClickPropagation(container);
            L.DomEvent.disableScrollPropagation(container);

            container.innerHTML =
                '<div class="geosearch-bar">' +
                '  <span class="geosearch-icon" title="Wyszukaj miejscowość">' +
                '    <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="#555" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>' +
                '  </span>' +
                '  <input type="text" class="geosearch-input" placeholder="' + this.options.placeholder + '" autocomplete="off" spellcheck="false" />' +
                '  <span class="geosearch-spinner" style="display:none;"></span>' +
                '  <button type="button" class="geosearch-clear" title="Wyczyść" style="display:none;">' +
                '    <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="#555" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><line x1="18" y1="6" x2="6" y2="18"></line><line x1="6" y1="6" x2="18" y2="18"></line></svg>' +
                '  </button>' +
                '</div>' +
                '<ul class="geosearch-results" style="display:none;"></ul>';

            var input = container.querySelector('.geosearch-input');
            var clearBtn = container.querySelector('.geosearch-clear');
            var spinner = container.querySelector('.geosearch-spinner');
            var resultsList = container.querySelector('.geosearch-results');

            var debounceTimer = null;
            var currentResults = [];
            var selectedIndex = -1;
            var activeScript = null;

            function hideResults() {
                resultsList.style.display = 'none';
                resultsList.innerHTML = '';
                currentResults = [];
                selectedIndex = -1;
            }

            function selectResult(index) {
                if (index < 0 || index >= currentResults.length) return;
                var item = currentResults[index];

                if (typeof options.onSelect === 'function') {
                    options.onSelect(item, mapInstance);
                } else {
                    if (item.boundingbox && item.boundingbox.length === 4) {
                        var south = parseFloat(item.boundingbox[0]);
                        var north = parseFloat(item.boundingbox[1]);
                        var west = parseFloat(item.boundingbox[2]);
                        var east = parseFloat(item.boundingbox[3]);
                        mapInstance.fitBounds([[south, west], [north, east]], { maxZoom: 15 });
                    } else if (item.lat && item.lon) {
                        mapInstance.setView([parseFloat(item.lat), parseFloat(item.lon)], 14);
                    }
                }

                var parts = (item.display_name || '').split(',');
                input.value = parts.length > 0 ? parts[0].trim() : item.display_name;
                hideResults();
                input.blur();
            }

            function updateSelectedHighlight() {
                var items = resultsList.querySelectorAll('li');
                for (var i = 0; i < items.length; i++) {
                    if (i === selectedIndex) {
                        items[i].classList.add('selected');
                        items[i].scrollIntoView({ block: 'nearest' });
                    } else {
                        items[i].classList.remove('selected');
                    }
                }
            }

            function performSearch(query, autoSelectFirst) {
                query = (query || '').trim();
                if (query.length < 2) {
                    hideResults();
                    spinner.style.display = 'none';
                    return;
                }

                spinner.style.display = 'inline-block';

                if (activeScript && activeScript.parentNode) {
                    activeScript.parentNode.removeChild(activeScript);
                    activeScript = null;
                }

                var cbName = 'geoSearchCb_' + Math.floor(Math.random() * 1000000);
                window[cbName] = function(data) {
                    delete window[cbName];
                    if (activeScript && activeScript.parentNode) {
                        activeScript.parentNode.removeChild(activeScript);
                        activeScript = null;
                    }

                    spinner.style.display = 'none';
                    currentResults = data || [];
                    selectedIndex = -1;
                    resultsList.innerHTML = '';

                    if (autoSelectFirst && currentResults.length > 0) {
                        selectResult(0);
                        return;
                    }

                    if (currentResults.length === 0) {
                        var noResLi = document.createElement('li');
                        noResLi.className = 'geosearch-no-results';
                        noResLi.textContent = 'Nie znaleziono miejscowości';
                        resultsList.appendChild(noResLi);
                        resultsList.style.display = 'block';
                        return;
                    }

                    currentResults.forEach(function(item, idx) {
                        var li = document.createElement('li');
                        var parts = (item.display_name || '').split(',');
                        var mainName = parts[0].trim();
                        var subName = parts.slice(1).join(',').trim();

                        var mainSpan = document.createElement('strong');
                        mainSpan.textContent = mainName;
                        li.appendChild(mainSpan);

                        if (subName) {
                            var subDiv = document.createElement('div');
                            subDiv.className = 'geosearch-item-type';
                            subDiv.textContent = subName;
                            li.appendChild(subDiv);
                        }

                        li.addEventListener('click', function(ev) {
                            ev.stopPropagation();
                            selectResult(idx);
                        });

                        resultsList.appendChild(li);
                    });

                    resultsList.style.display = 'block';
                };

                var url = 'https://nominatim.openstreetmap.org/search?format=json' +
                          '&q=' + encodeURIComponent(query) +
                          '&accept-language=pl' +
                          '&limit=6' +
                          '&json_callback=' + cbName;

                activeScript = document.createElement('script');
                activeScript.src = url;
                activeScript.onerror = function() {
                    delete window[cbName];
                    spinner.style.display = 'none';
                    if (activeScript && activeScript.parentNode) {
                        activeScript.parentNode.removeChild(activeScript);
                        activeScript = null;
                    }
                    resultsList.innerHTML = '<li class="geosearch-no-results">Błąd wyszukiwania lub brak połączenia</li>';
                    resultsList.style.display = 'block';
                };
                document.body.appendChild(activeScript);
            }

            input.addEventListener('input', function() {
                var val = input.value.trim();
                clearBtn.style.display = val.length > 0 ? 'inline-flex' : 'none';

                if (debounceTimer) clearTimeout(debounceTimer);
                if (val.length < 2) {
                    hideResults();
                    return;
                }

                debounceTimer = setTimeout(function() {
                    performSearch(val, false);
                }, 350);
            });

            input.addEventListener('keydown', function(e) {
                if (e.key === 'ArrowDown') {
                    if (resultsList.style.display !== 'none' && currentResults.length > 0) {
                        selectedIndex = (selectedIndex + 1) % currentResults.length;
                        updateSelectedHighlight();
                        e.preventDefault();
                    }
                } else if (e.key === 'ArrowUp') {
                    if (resultsList.style.display !== 'none' && currentResults.length > 0) {
                        selectedIndex = (selectedIndex - 1 + currentResults.length) % currentResults.length;
                        updateSelectedHighlight();
                        e.preventDefault();
                    }
                } else if (e.key === 'Enter') {
                    e.preventDefault();
                    if (selectedIndex >= 0 && selectedIndex < currentResults.length) {
                        selectResult(selectedIndex);
                    } else if (currentResults.length > 0) {
                        selectResult(0);
                    } else {
                        if (debounceTimer) clearTimeout(debounceTimer);
                        performSearch(input.value, true);
                    }
                } else if (e.key === 'Escape') {
                    hideResults();
                    input.blur();
                }
            });

            clearBtn.addEventListener('click', function(e) {
                e.stopPropagation();
                input.value = '';
                clearBtn.style.display = 'none';
                hideResults();
                input.focus();
            });

            document.addEventListener('click', function(e) {
                if (!container.contains(e.target)) {
                    hideResults();
                }
            });

            return container;
        }
    });

    new GeoSearchControl().addTo(mapInstance);
}
