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
});
