// saleForm.js - Versión final
console.log('saleForm.js cargado');

$(document).ready(function () {
    let detailIndex = 0;
    let products = {};
    let stock = {};
    let productsOptions = '';

    // Cargar datos
    const dataScript = document.getElementById('saleFormData');
    if (dataScript) {
        const data = JSON.parse(dataScript.textContent);
        products = data.products;
        stock = data.stock;
        productsOptions = data.productsOptions;
    }

    // Botón agregar producto
    $('#btnAddProduct').click(function () {
        $('#emptyMessage').hide();

        const html = `
    <div class="card mb-2 product-item">
        <div class="card-body p-2">
            <div class="row g-2 align-items-end">
                <div class="col-md-3">
                    <label class="form-label small mb-1">Producto</label>
                    <select name="Details[${detailIndex}].ProductId" class="form-select form-select-sm product-select" required>
                        <option value="">Seleccionar...</option>
                        ${productsOptions}
                    </select>
                </div>
                <div class="col-md-2">
                    <label class="form-label small mb-1">Stock</label>
                    <input type="text" class="form-control form-control-sm stock-display bg-light text-center" readonly value="0" />
                </div>
                <div class="col-md-2">
                    <label class="form-label small mb-1">Cant.</label>
                    <input type="number" name="Details[${detailIndex}].Quantity" value="1" class="form-control form-control-sm quantity-input" min="1" required />
                </div>
                <div class="col-md-2">
                    <label class="form-label small mb-1">Precio</label>
                    <input type="number" name="Details[${detailIndex}].UnitPrice" value="0" class="form-control form-control-sm price-input" step="0.01" min="0.01" required readonly />
                </div>
                <div class="col-md-2">
                    <label class="form-label small mb-1">Subtotal</label>
                    <input type="text" class="form-control form-control-sm subtotal-display bg-light text-end fw-bold" readonly value="$0.00" />
                </div>
                <div class="col-md-1">
                    <button type="button" class="btn btn-sm btn-danger w-100 btn-remove">
                        <i class="bi bi-trash"></i>
                    </button>
                </div>
            </div>
        </div>
    </div>
`;

        $('#productsContainer').append(html);
        detailIndex++;
        updateTotal();
    });

    // Eliminar producto
    $(document).on('click', '.btn-remove', function () {
        $(this).closest('.product-item').remove();
        updateTotal();
    });

    // Cargar precio y stock al seleccionar producto
    $(document).on('change', '.product-select', function () {
        const productId = $(this).val();
        const item = $(this).closest('.product-item');
        const priceInput = item.find('.price-input');
        const stockDisplay = item.find('.stock-display');
        const quantityInput = item.find('.quantity-input');

        if (productId && products[productId]) {
            priceInput.val(products[productId].toFixed(2));

            const availableStock = stock[productId] || 0;
            stockDisplay.val(availableStock);
            quantityInput.attr('max', availableStock);

            if (availableStock === 0) {
                alert('Este producto no tiene stock disponible');
                quantityInput.val(0);
            } else if (parseInt(quantityInput.val()) > availableStock) {
                quantityInput.val(availableStock);
            }

            calcSubtotal(item);
        } else {
            priceInput.val(0);
            stockDisplay.val(0);
            quantityInput.attr('max', 0);
            calcSubtotal(item);
        }
    });

    // Calcular subtotal al cambiar cantidad o precio
    $(document).on('input', '.quantity-input, .price-input', function () {
        const item = $(this).closest('.product-item');
        const quantity = parseInt(item.find('.quantity-input').val()) || 0;
        const maxStock = parseInt(item.find('.stock-display').val()) || 0;

        if (quantity > maxStock && maxStock > 0) {
            alert(`Stock insuficiente. Disponible: ${maxStock}`);
            item.find('.quantity-input').val(maxStock);
        }

        calcSubtotal(item);
    });

    function calcSubtotal(item) {
        const qty = parseFloat(item.find('.quantity-input').val()) || 0;
        const price = parseFloat(item.find('.price-input').val()) || 0;
        const subtotal = qty * price;
        item.find('.subtotal-display').val('$' + subtotal.toFixed(2));
        updateTotal();
    }

    function updateTotal() {
        let total = 0;
        let count = 0;

        $('.product-item').each(function () {
            const qty = parseFloat($(this).find('.quantity-input').val()) || 0;
            const price = parseFloat($(this).find('.price-input').val()) || 0;
            if (qty > 0 && price > 0) {
                total += qty * price;
                count++;
            }
        });

        $('#totalAmount').text('$' + total.toFixed(2));
        $('#itemCount').text(count);

        if (count === 0) {
            $('#emptyMessage').show();
        }
    }

    // Validar antes de enviar
    $('#saleForm').submit(function (e) {
        if ($('.product-item').length === 0) {
            e.preventDefault();
            alert('Debe agregar al menos un producto a la venta');
            return false;
        }
    });
});