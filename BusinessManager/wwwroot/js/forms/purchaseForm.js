// purchaseForm.js
var PurchaseForm = (function () {
    let detailIndex = 0;
    let products = {};
    let productsOptions = '';

    function init() {
        // Leer datos del script JSON
        const dataScript = document.getElementById('purchaseFormData');
        if (dataScript) {
            const data = JSON.parse(dataScript.textContent);
            detailIndex = data.detailCount;
            products = data.products;
            productsOptions = data.productsOptions;
        }

        // Event listeners
        $('#btnAddDetail').off('click').on('click', addDetail);
        $(document).off('click', '.btn-remove-detail').on('click', '.btn-remove-detail', removeDetail);
        $(document).off('change', '.product-select').on('change', '.product-select', function () {
            loadProductPrice(this);
        });
        $(document).off('input', '.quantity-input, .price-input').on('input', '.quantity-input, .price-input', function () {
            calculateSubtotal(this);
        });

        // Validación
        if ($.validator && $.validator.unobtrusive) {
            $.validator.unobtrusive.parse("#purchaseForm");
        }

        // Calcular subtotales iniciales (para edición)
        calculateAllSubtotals();
    }

    function addDetail() {
        const html = `
            <div class="card mb-2 detail-item">
                <div class="card-body p-2">
                    <div class="row g-2">
                        <div class="col-md-4">
                            <select name="Details[${detailIndex}].ProductId" class="form-select form-select-sm product-select" required>
                                <option value="">Producto...</option>
                                ${productsOptions}
                            </select>
                        </div>
                        <div class="col-md-2">
                            <input type="number" name="Details[${detailIndex}].Quantity" value="1" class="form-control form-control-sm quantity-input" placeholder="Cant." min="1" required />
                        </div>
                        <div class="col-md-2">
                            <input type="number" name="Details[${detailIndex}].UnitPrice" value="0" class="form-control form-control-sm price-input" placeholder="Precio" step="0.01" min="0.01" required />
                        </div>
                        <div class="col-md-2">
                            <input type="text" class="form-control form-control-sm subtotal-display" readonly placeholder="Subtotal" />
                        </div>
                        <div class="col-md-2">
                            <button type="button" class="btn btn-sm btn-danger w-100 btn-remove-detail">
                                <i class="bi bi-trash"></i>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        `;
        $('#detailsContainer').append(html);
        detailIndex++;
    }

    function loadProductPrice(selectElement) {
        const productId = $(selectElement).val();
        const row = $(selectElement).closest('.detail-item');
        const priceInput = row.find('.price-input');

        if (productId && products[productId]) {
            priceInput.val(products[productId].toFixed(2));
            calculateSubtotal(priceInput[0]);
        } else {
            priceInput.val(0);
            calculateSubtotal(priceInput[0]);
        }
    }

    function calculateSubtotal(element) {
        const row = $(element).closest('.detail-item');
        const quantity = parseFloat(row.find('.quantity-input').val()) || 0;
        const price = parseFloat(row.find('.price-input').val()) || 0;
        const subtotal = quantity * price;

        row.find('.subtotal-display').val('$' + subtotal.toFixed(2));
        calculateTotal();
    }

    function calculateTotal() {
        let total = 0;
        $('.detail-item').each(function () {
            const quantity = parseFloat($(this).find('.quantity-input').val()) || 0;
            const price = parseFloat($(this).find('.price-input').val()) || 0;
            total += quantity * price;
        });
        $('#totalAmount').text('$' + total.toFixed(2));
    }

    function calculateAllSubtotals() {
        $('.detail-item').each(function () {
            const priceInput = $(this).find('.price-input')[0];
            if (priceInput) {
                calculateSubtotal(priceInput);
            }
        });
    }

    function removeDetail() {
        $(this).closest('.detail-item').remove();
        calculateTotal();
    }

    return {
        init: init
    };
})();

// Inicializar cuando el modal se abre
$(document).on('shown.bs.modal', '#purchaseModal', function () {
    PurchaseForm.init();
});