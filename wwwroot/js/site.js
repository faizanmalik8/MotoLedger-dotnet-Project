// ============================================================
// MotoLedger - Site JavaScript
// ============================================================

document.addEventListener('DOMContentLoaded', function () {

    // --- Password Visibility Toggle ---
    document.querySelectorAll('.password-toggle').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var input = this.parentElement.querySelector('input');
            var icon = this.querySelector('i');
            if (input.type === 'password') {
                input.type = 'text';
                icon.classList.remove('bi-eye');
                icon.classList.add('bi-eye-slash');
            } else {
                input.type = 'password';
                icon.classList.remove('bi-eye-slash');
                icon.classList.add('bi-eye');
            }
        });
    });

    // --- Active Nav Link Highlight ---
    var currentPath = window.location.pathname.toLowerCase();
    document.querySelectorAll('.ml-navbar .nav-link').forEach(function (link) {
        var href = link.getAttribute('href');
        if (href && href !== '#') {
            var linkPath = href.toLowerCase();
            if (currentPath === linkPath || (linkPath !== '/' && currentPath.startsWith(linkPath))) {
                link.classList.add('active');
            }
        }
    });

    // --- Animate Elements on Scroll ---
    var animateElements = document.querySelectorAll('.animate-in');
    if (animateElements.length > 0) {
        var observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    entry.target.style.animationPlayState = 'running';
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.1 });

        animateElements.forEach(function (el) {
            observer.observe(el);
        });
    }

    // --- Navbar Scroll Effect ---
    var navbar = document.querySelector('.ml-navbar');
    if (navbar) {
        window.addEventListener('scroll', function () {
            if (window.scrollY > 20) {
                navbar.style.boxShadow = '0 8px 40px rgba(0, 0, 0, 0.4)';
            } else {
                navbar.style.boxShadow = '0 4px 30px rgba(0, 0, 0, 0.3)';
            }
        });
    }

    // --- Form validation visual feedback ---
    document.querySelectorAll('.ml-form-control').forEach(function (input) {
        input.addEventListener('blur', function () {
            if (this.value.trim() !== '' && this.checkValidity()) {
                this.classList.remove('input-error');
                this.classList.add('input-success');
            }
        });

        input.addEventListener('invalid', function () {
            this.classList.add('input-error');
            this.classList.remove('input-success');
        });
    });

    // --- Counter Animation for KPI values ---
    function animateCounter(element, target, duration) {
        var start = 0;
        var startTime = null;
        var prefix = element.dataset.prefix || '';
        var suffix = element.dataset.suffix || '';

        function step(timestamp) {
            if (!startTime) startTime = timestamp;
            var progress = Math.min((timestamp - startTime) / duration, 1);
            var eased = 1 - Math.pow(1 - progress, 3);
            var current = Math.floor(eased * target);
            element.textContent = prefix + current.toLocaleString() + suffix;
            if (progress < 1) {
                window.requestAnimationFrame(step);
            } else {
                element.textContent = prefix + target.toLocaleString() + suffix;
            }
        }
        window.requestAnimationFrame(step);
    }

    document.querySelectorAll('[data-counter]').forEach(function (el) {
        var target = parseInt(el.dataset.counter, 10);
        var duration = parseInt(el.dataset.duration || '1500', 10);
        var counterObserver = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    animateCounter(el, target, duration);
                    counterObserver.unobserve(el);
                }
            });
        }, { threshold: 0.5 });
        counterObserver.observe(el);
    });

    // --- Toast auto-dismiss ---
    document.querySelectorAll('.toast').forEach(function (toast) {
        var bsToast = new bootstrap.Toast(toast, { delay: 4000 });
        bsToast.show();
    });
    // ============================================================
    // AJAX: Vehicle Inventory Live Search & Filter
    // ============================================================
    if ($('#vehicleTableBody').length > 0) {
        var searchDebounceTimer;

        function performVehicleSearch() {
            var searchTerm = $('#vehicleSearchInput').val();
            var statusTerm = $('#vehicleStatusFilter').val();

            $.ajax({
                url: '/Vehicle/Search',
                type: 'GET',
                data: {
                    term: searchTerm,
                    status: statusTerm
                },
                beforeSend: function () {
                    // Slight fade to indicate loading
                    $('#vehicleTableBody').css('opacity', '0.5');
                },
                success: function (responseHtml) {
                    // Update table rows without refreshing the page
                    $('#vehicleTableBody').html(responseHtml);
                    $('#vehicleTableBody').css('opacity', '1');
                },
                error: function (xhr, status, error) {
                    console.error('Vehicle search error:', error);
                    $('#vehicleTableBody').css('opacity', '1');
                }
            });
        }

        // Trigger search on typing (with 300ms debounce so it waits until typing pauses)
        $('#vehicleSearchInput').on('keyup input', function () {
            clearTimeout(searchDebounceTimer);
            searchDebounceTimer = setTimeout(performVehicleSearch, 300);
        });

        // Trigger search immediately when dropdown option changes
        $('#vehicleStatusFilter').on('change', function () {
            performVehicleSearch();
        });
    }
    // ============================================================
    // AJAX (JSON): Live Vehicle Financials & Margin Calculation
    // ============================================================
    if ($('#vehicleSelect').length > 0) {
        var currentTotalCost = 0;

        function calculateProfit() {
            var salePrice = parseFloat($('#salePriceInput').val()) || 0;
            $('#lblAgreedPrice').text('Rs. ' + salePrice.toLocaleString());

            if (currentTotalCost > 0 && salePrice > 0) {
                var profit = salePrice - currentTotalCost;
                var margin = ((profit / salePrice) * 100).toFixed(1);

                if (profit >= 0) {
                    $('#lblNetProfit').text('+Rs. ' + Math.round(profit).toLocaleString())
                        .css('color', '#1E5C3A');
                    $('#lblMarginBadge').text(margin + '% Margin')
                        .css('background-color', '#1E5C3A');
                } else {
                    $('#lblNetProfit').text('-Rs. ' + Math.abs(Math.round(profit)).toLocaleString())
                        .css('color', '#C0392B');
                    $('#lblMarginBadge').text(margin + '% Loss')
                        .css('background-color', '#C0392B');
                }
            }
        }

        function fetchVehicleFinancials(vehicleId) {
            if (!vehicleId) {
                currentTotalCost = 0;
                $('#basePurchasePrice').text('-Rs. 0');
                $('#lblWorkshopExpenses').text('-Rs. 0');
                $('#lblTotalCost').text('Rs. 0');
                $('#lblNetProfit').text('Rs. 0');
                $('#lblMarginBadge').text('0.0% Margin');
                return;
            }

            $.ajax({
                url: '/Sales/GetVehicleFinancials',
                type: 'GET',
                data: { vehicleId: vehicleId },
                dataType: 'json', // Expecting raw JSON response from Controller
                success: function (data) {
                    if (data.success) {
                        currentTotalCost = parseFloat(data.totalcost) || 0;

                        // Update the breakdown card with JSON values from the database
                        $('#basePurchasePrice').text('-Rs. ' + Number(data.baseprice).toLocaleString());
                        $('#lblWorkshopExpenses').text('-Rs. ' + Number(data.workshopExpenses).toLocaleString());
                        $('#lblTotalCost').text('Rs. ' + Number(data.totalcost).toLocaleString());

                        // Recalculate profit margin in real time
                        calculateProfit();
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error fetching vehicle financials:', error);
                }
            });
        }

        // When user picks a vehicle from the dropdown
        $('#vehicleSelect').on('change', function () {
            var selectedId = $(this).val();
            fetchVehicleFinancials(selectedId);
        });

        // When user types or changes the sale price
        $('#salePriceInput').on('keyup input', function () {
            calculateProfit();
        });

        // If a vehicle is already preselected on page load, trigger calculation
        if ($('#vehicleSelect').val()) {
            fetchVehicleFinancials($('#vehicleSelect').val());
        }
    }
});
