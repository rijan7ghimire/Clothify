// ============================================
// CLOTHIFY — Core JavaScript
// ============================================

const Clothify = {
    // API base URL
    apiBase: '',

    // Toast notification
    toast(message, type = 'success', duration = 3000) {
        const existing = document.querySelector('.toast');
        if (existing) existing.remove();

        const toast = document.createElement('div');
        toast.className = `toast ${type}`;
        toast.textContent = message;
        document.body.appendChild(toast);

        requestAnimationFrame(() => toast.classList.add('show'));
        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => toast.remove(), 300);
        }, duration);
    },

    // Accordion toggle
    initAccordions() {
        document.querySelectorAll('.accordion__header').forEach(header => {
            header.addEventListener('click', () => {
                const item = header.parentElement;
                const wasOpen = item.classList.contains('open');
                // Close others in same accordion
                item.closest('.accordion')?.querySelectorAll('.accordion__item').forEach(i => {
                    i.classList.remove('open');
                });
                if (!wasOpen) item.classList.add('open');
            });
        });
    },

    // Bottom sheet
    openBottomSheet(id) {
        const sheet = document.getElementById(id);
        const backdrop = document.getElementById(id + '-backdrop');
        if (sheet) sheet.classList.add('show');
        if (backdrop) backdrop.classList.add('show');
        document.body.classList.add('overflow-hidden');
    },

    closeBottomSheet(id) {
        const sheet = document.getElementById(id);
        const backdrop = document.getElementById(id + '-backdrop');
        if (sheet) sheet.classList.remove('show');
        if (backdrop) backdrop.classList.remove('show');
        document.body.classList.remove('overflow-hidden');
    },

    // Wishlist toggle
    toggleWishlist(btn, productId) {
        btn.classList.toggle('active');
        const isActive = btn.classList.contains('active');
        btn.style.animation = 'heartBounce 0.3s ease';
        setTimeout(() => btn.style.animation = '', 300);

        this.toast(isActive ? 'Added to wishlist' : 'Removed from wishlist');
    },

    // Quantity selector
    initQuantitySelectors() {
        document.querySelectorAll('.qty-selector:not([data-manual])').forEach(selector => {
            const minus = selector.querySelector('.qty-minus');
            const plus = selector.querySelector('.qty-plus');
            const count = selector.querySelector('.qty-selector__count');

            minus?.addEventListener('click', () => {
                let val = parseInt(count.textContent);
                if (val > 1) {
                    count.textContent = val - 1;
                    minus.disabled = val - 1 <= 1;
                    selector.dispatchEvent(new CustomEvent('change', { detail: { quantity: val - 1 } }));
                }
            });

            plus?.addEventListener('click', () => {
                let val = parseInt(count.textContent);
                if (val < 10) {
                    count.textContent = val + 1;
                    minus.disabled = false;
                    selector.dispatchEvent(new CustomEvent('change', { detail: { quantity: val + 1 } }));
                }
            });
        });
    },

    // Chip selection
    initChips() {
        document.querySelectorAll('[data-chip-group]').forEach(group => {
            const multi = group.dataset.chipMulti === 'true';
            group.querySelectorAll('.chip').forEach(chip => {
                chip.addEventListener('click', () => {
                    if (!multi) {
                        group.querySelectorAll('.chip').forEach(c => c.classList.remove('selected'));
                    }
                    chip.classList.toggle('selected');
                });
            });
        });
    },

    // Color picker
    initColorPickers() {
        document.querySelectorAll('[data-color-group]').forEach(group => {
            group.querySelectorAll('.color-circle').forEach(circle => {
                circle.addEventListener('click', () => {
                    group.querySelectorAll('.color-circle').forEach(c => c.classList.remove('selected'));
                    circle.classList.add('selected');
                });
            });
        });
    },

    // Simple image carousel
    initCarousels() {
        document.querySelectorAll('[data-carousel]').forEach(carousel => {
            const track = carousel.querySelector('.carousel-track');
            const dots = carousel.querySelectorAll('.dot');
            let current = 0;
            const total = dots.length;

            const goTo = (index) => {
                current = index;
                track.style.transform = `translateX(-${current * 100}%)`;
                dots.forEach((d, i) => d.classList.toggle('active', i === current));
            };

            dots.forEach((dot, i) => dot.addEventListener('click', () => goTo(i)));

            // Auto-rotate
            if (carousel.dataset.autoRotate) {
                setInterval(() => goTo((current + 1) % total), 4000);
            }

            // Swipe support
            let startX = 0;
            track?.addEventListener('touchstart', e => startX = e.touches[0].clientX);
            track?.addEventListener('touchend', e => {
                const diff = startX - e.changedTouches[0].clientX;
                if (Math.abs(diff) > 50) {
                    goTo(diff > 0 ? Math.min(current + 1, total - 1) : Math.max(current - 1, 0));
                }
            });
        });
    },

    // Password toggle
    initPasswordToggles() {
        document.querySelectorAll('[data-toggle-password]').forEach(btn => {
            btn.addEventListener('click', () => {
                const input = btn.closest('.form-input-icon').querySelector('input');
                const isPassword = input.type === 'password';
                input.type = isPassword ? 'text' : 'password';
                btn.innerHTML = isPassword
                    ? '<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M17.94 17.94A10.07 10.07 0 0112 20c-7 0-11-8-11-8a18.45 18.45 0 015.06-5.94M9.9 4.24A9.12 9.12 0 0112 4c7 0 11 8 11 8a18.5 18.5 0 01-2.16 3.19m-6.72-1.07a3 3 0 11-4.24-4.24"/><line x1="1" y1="1" x2="23" y2="23"/></svg>'
                    : '<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>';
            });
        });
    },

    // Initialize all components
    init() {
        this.initAccordions();
        this.initQuantitySelectors();
        this.initChips();
        this.initColorPickers();
        this.initCarousels();
        this.initPasswordToggles();
    }
};

document.addEventListener('DOMContentLoaded', () => Clothify.init());
