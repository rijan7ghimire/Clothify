/* =========================================
   Clothify - JavaScript Utilities
   Mobile-First E-Commerce for Nepal
   ========================================= */

(function () {
    "use strict";

    /* -----------------------------------------
       Toast Notifications
       ----------------------------------------- */
    function showToast(message, type) {
        type = type || "success";
        var container = document.getElementById("toast-container");
        if (!container) {
            container = document.createElement("div");
            container.id = "toast-container";
            container.style.cssText =
                "position:fixed;top:70px;left:50%;transform:translateX(-50%);" +
                "z-index:9999;width:90%;max-width:400px;pointer-events:none;";
            document.body.appendChild(container);
        }

        var toast = document.createElement("div");
        toast.className = "toast toast-" + type;

        var bgColor = "#28A745";
        if (type === "error") bgColor = "#DC3545";
        if (type === "warning") bgColor = "#FFC107";
        if (type === "info") bgColor = "#17A2B8";

        var textColor = type === "warning" ? "#333" : "#fff";

        toast.style.cssText =
            "background:" + bgColor + ";color:" + textColor + ";" +
            "padding:12px 16px;border-radius:8px;margin-bottom:8px;" +
            "font-size:14px;font-weight:500;box-shadow:0 4px 12px rgba(0,0,0,0.15);" +
            "opacity:0;transition:opacity 0.3s ease,transform 0.3s ease;" +
            "transform:translateY(-10px);pointer-events:auto;";

        toast.textContent = message;
        container.appendChild(toast);

        // Trigger animation
        requestAnimationFrame(function () {
            toast.style.opacity = "1";
            toast.style.transform = "translateY(0)";
        });

        // Auto-dismiss after 3 seconds
        setTimeout(function () {
            toast.style.opacity = "0";
            toast.style.transform = "translateY(-10px)";
            setTimeout(function () {
                if (toast.parentNode) {
                    toast.parentNode.removeChild(toast);
                }
            }, 300);
        }, 3000);
    }

    // Expose globally
    window.showToast = showToast;

    /* -----------------------------------------
       Quantity Controls (Cart Page)
       ----------------------------------------- */
    function incrementQuantity(inputId) {
        var input = document.getElementById(inputId);
        if (input) {
            var currentVal = parseInt(input.value, 10) || 0;
            if (currentVal < 99) {
                input.value = currentVal + 1;
                triggerChange(input);
            }
        }
    }

    function decrementQuantity(inputId) {
        var input = document.getElementById(inputId);
        if (input) {
            var currentVal = parseInt(input.value, 10) || 0;
            if (currentVal > 1) {
                input.value = currentVal - 1;
                triggerChange(input);
            }
        }
    }

    function triggerChange(element) {
        var event;
        if (typeof Event === "function") {
            event = new Event("change", { bubbles: true });
        } else {
            event = document.createEvent("Event");
            event.initEvent("change", true, true);
        }
        element.dispatchEvent(event);
    }

    window.incrementQuantity = incrementQuantity;
    window.decrementQuantity = decrementQuantity;

    /* -----------------------------------------
       Rating Button Selection (Feedback Page)
       ----------------------------------------- */
    function initRatingButtons() {
        var ratingContainers = document.querySelectorAll(".rating-buttons");
        ratingContainers.forEach(function (container) {
            var buttons = container.querySelectorAll(".rating-btn");
            buttons.forEach(function (btn) {
                btn.addEventListener("click", function (e) {
                    e.preventDefault();
                    // Remove selected class from all buttons in this container
                    buttons.forEach(function (b) {
                        b.classList.remove("selected");
                    });
                    // Add selected class to clicked button
                    btn.classList.add("selected");

                    // Update hidden field if present
                    var hiddenFieldId = container.getAttribute("data-hidden-field");
                    if (hiddenFieldId) {
                        var hiddenField = document.getElementById(hiddenFieldId);
                        if (hiddenField) {
                            hiddenField.value = btn.getAttribute("data-value") || btn.textContent.trim();
                        }
                    }
                });
            });
        });
    }

    /* -----------------------------------------
       Category Tab Click Handler
       ----------------------------------------- */
    function initCategoryTabs() {
        var tabContainers = document.querySelectorAll(".category-tabs, .filter-tabs");
        tabContainers.forEach(function (container) {
            var tabs = container.querySelectorAll(".category-tab, .filter-tab");
            tabs.forEach(function (tab) {
                tab.addEventListener("click", function (e) {
                    // Only prevent default if it's not a real link with an href
                    if (!tab.getAttribute("href") || tab.getAttribute("href") === "#") {
                        e.preventDefault();
                    }
                    // Remove active class from all tabs in this container
                    tabs.forEach(function (t) {
                        t.classList.remove("active");
                    });
                    // Add active class to clicked tab
                    tab.classList.add("active");
                });
            });
        });
    }

    /* -----------------------------------------
       Search Input Handler
       ----------------------------------------- */
    function initSearchBar() {
        var searchInputs = document.querySelectorAll(".search-bar");
        searchInputs.forEach(function (input) {
            var debounceTimer = null;
            input.addEventListener("input", function () {
                clearTimeout(debounceTimer);
                debounceTimer = setTimeout(function () {
                    var query = input.value.trim().toLowerCase();
                    var targetContainerId = input.getAttribute("data-target");

                    if (targetContainerId) {
                        var container = document.getElementById(targetContainerId);
                        if (container) {
                            var items = container.querySelectorAll("[data-search-text]");
                            items.forEach(function (item) {
                                var text = (item.getAttribute("data-search-text") || "").toLowerCase();
                                if (query === "" || text.indexOf(query) !== -1) {
                                    item.style.display = "";
                                } else {
                                    item.style.display = "none";
                                }
                            });
                        }
                    }
                }, 300);
            });
        });
    }

    /* -----------------------------------------
       Mobile Menu Toggle
       ----------------------------------------- */
    function initMobileMenu() {
        var menuToggle = document.getElementById("menu-toggle");
        var sideNav = document.querySelector(".admin-side-nav");

        if (menuToggle && sideNav) {
            menuToggle.addEventListener("click", function (e) {
                e.preventDefault();
                sideNav.classList.toggle("open");
            });
        }
    }

    /* -----------------------------------------
       Form Validation Helpers
       ----------------------------------------- */
    function validateRequired(fieldId, errorMessage) {
        var field = document.getElementById(fieldId);
        if (!field) return false;

        var value = field.value.trim();
        if (value === "") {
            showFieldError(field, errorMessage || "This field is required.");
            return false;
        }
        clearFieldError(field);
        return true;
    }

    function validateEmail(fieldId) {
        var field = document.getElementById(fieldId);
        if (!field) return false;

        var value = field.value.trim();
        var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailPattern.test(value)) {
            showFieldError(field, "Please enter a valid email address.");
            return false;
        }
        clearFieldError(field);
        return true;
    }

    function validatePhone(fieldId) {
        var field = document.getElementById(fieldId);
        if (!field) return false;

        var value = field.value.trim().replace(/[\s\-]/g, "");
        // Nepal phone numbers: 98XXXXXXXX or 97XXXXXXXX (10 digits)
        var phonePattern = /^(98|97)\d{8}$/;
        if (!phonePattern.test(value)) {
            showFieldError(field, "Please enter a valid Nepal phone number (98/97XXXXXXXX).");
            return false;
        }
        clearFieldError(field);
        return true;
    }

    function validateMinLength(fieldId, minLen) {
        var field = document.getElementById(fieldId);
        if (!field) return false;

        if (field.value.trim().length < minLen) {
            showFieldError(field, "Must be at least " + minLen + " characters.");
            return false;
        }
        clearFieldError(field);
        return true;
    }

    function validatePasswordMatch(passwordFieldId, confirmFieldId) {
        var pass = document.getElementById(passwordFieldId);
        var confirm = document.getElementById(confirmFieldId);
        if (!pass || !confirm) return false;

        if (pass.value !== confirm.value) {
            showFieldError(confirm, "Passwords do not match.");
            return false;
        }
        clearFieldError(confirm);
        return true;
    }

    function showFieldError(field, message) {
        clearFieldError(field);
        field.style.borderColor = "#DC3545";
        var errorEl = document.createElement("div");
        errorEl.className = "field-error";
        errorEl.style.cssText = "color:#DC3545;font-size:12px;margin-top:4px;";
        errorEl.textContent = message;
        field.parentNode.appendChild(errorEl);
    }

    function clearFieldError(field) {
        field.style.borderColor = "";
        var existing = field.parentNode.querySelector(".field-error");
        if (existing) {
            existing.parentNode.removeChild(existing);
        }
    }

    window.validateRequired = validateRequired;
    window.validateEmail = validateEmail;
    window.validatePhone = validatePhone;
    window.validateMinLength = validateMinLength;
    window.validatePasswordMatch = validatePasswordMatch;

    /* -----------------------------------------
       Add to Cart Animation / Feedback
       ----------------------------------------- */
    function addToCartFeedback(buttonElement) {
        if (!buttonElement) return;

        var originalText = buttonElement.textContent;
        var originalBg = buttonElement.style.backgroundColor;

        buttonElement.textContent = "Added!";
        buttonElement.style.backgroundColor = "#28A745";
        buttonElement.disabled = true;

        // Animate a small scale bounce
        buttonElement.style.transition = "transform 0.15s ease";
        buttonElement.style.transform = "scale(1.05)";
        setTimeout(function () {
            buttonElement.style.transform = "scale(1)";
        }, 150);

        // Update cart badge count
        var cartBadge = document.querySelector(".cart-badge");
        if (cartBadge) {
            var count = parseInt(cartBadge.textContent, 10) || 0;
            cartBadge.textContent = count + 1;
            cartBadge.style.display = "flex";
        }

        showToast("Item added to cart!", "success");

        // Restore button after short delay
        setTimeout(function () {
            buttonElement.textContent = originalText;
            buttonElement.style.backgroundColor = originalBg || "";
            buttonElement.disabled = false;
        }, 1500);
    }

    window.addToCartFeedback = addToCartFeedback;

    /* -----------------------------------------
       Active Nav Highlight
       ----------------------------------------- */
    function highlightActiveNav() {
        var currentPath = window.location.pathname.toLowerCase();
        var navItems = document.querySelectorAll(".bottom-nav .nav-item");

        navItems.forEach(function (item) {
            var href = (item.getAttribute("href") || "").toLowerCase();
            // Extract the page name from the href
            var hrefPage = href.split("/").pop();
            var currentPage = currentPath.split("/").pop();

            if (hrefPage && currentPage && hrefPage === currentPage) {
                item.classList.add("active");
            } else {
                item.classList.remove("active");
            }
        });
    }

    /* -----------------------------------------
       Confirm Delete
       ----------------------------------------- */
    function confirmDelete(message) {
        return confirm(message || "Are you sure you want to delete this item?");
    }

    window.confirmDelete = confirmDelete;

    /* -----------------------------------------
       Image Preview for File Upload
       ----------------------------------------- */
    function previewImage(inputId, previewId) {
        var input = document.getElementById(inputId);
        var preview = document.getElementById(previewId);
        if (!input || !preview) return;

        input.addEventListener("change", function () {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    preview.src = e.target.result;
                    preview.style.display = "block";
                };
                reader.readAsDataURL(input.files[0]);
            }
        });
    }

    window.previewImage = previewImage;

    /* -----------------------------------------
       Initialize on DOM Ready
       ----------------------------------------- */
    document.addEventListener("DOMContentLoaded", function () {
        initRatingButtons();
        initCategoryTabs();
        initSearchBar();
        initMobileMenu();
        highlightActiveNav();
    });

})();
