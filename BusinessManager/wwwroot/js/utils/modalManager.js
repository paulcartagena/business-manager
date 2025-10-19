// modalManager.js
$(document).ready(function () {

    function openEntityModal(modalId, url) {
        $.ajax({
            url: url,
            type: 'GET',
            success: function (result) {
                $(`${modalId} .modal-content`).html(result);
                $(modalId).modal('show');
            },
            error: function (xhr, status, error) {
                console.error("Error al cargar el modal:", error);
                alert("Hubo un error al cargar el formulario.");
            }
        });
    }

    function handleFormSubmit(modalId, formSelector) {
        $(document).on('submit', formSelector, function (e) {
            e.preventDefault();
            var form = $(this);
            if (!form.valid()) return;

            $.ajax({
                url: form.attr('action'),
                type: form.attr('method'),
                data: form.serialize(),
                success: function (response) {
                    if (response.success) {
                        $(modalId).modal('hide');
                        alert(response.message);
                        location.reload();
                    } else {
                        $(`${modalId} .modal-content`).html(response);
                        if ($.validator && $.validator.unobtrusive) {
                            $.validator.unobtrusive.parse(formSelector);
                        }
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error al guardar:", error);
                    alert("Hubo un error al guardar.");
                }
            });
        });
    }

    // Entidades 
    const entities = [
        { createBtn: '#btnCreateProduct', modalId: '#productModal', formSelector: '#productForm', createUrl: '/Product/CreateProductModal', editBtn: '.btn-edit-product', editUrl: '/Product/EditProductModal?id=' },
        { createBtn: '#btnCreateUom', modalId: '#uomModal', formSelector: '#uomForm', createUrl: '/Uom/CreateUomModal', editBtn: '.btn-edit-uom', editUrl: '/Uom/EditUomModal?id=' },
        { createBtn: '#btnCreateCategory', modalId: '#categoryModal', formSelector: '#categoryForm', createUrl: '/Category/CreateCategoryModal', editBtn: '.btn-edit-category', editUrl: '/Category/EditCategoryModal?id=' },
        { createBtn: '#btnCreateUser', modalId: '#userModal', formSelector: '#userForm', createUrl: '/User/CreateUserModal', editBtn: '.btn-edit-user', editUrl: '/User/EditUserModal?id=' },
        { createBtn: '#btnCreateRol', modalId: '#rolModal', formSelector: '#rolForm', createUrl: '/Rol/CreateRolModal', editBtn: '.btn-edit-rol', editUrl: '/Rol/EditRolModal?id=' },
        { createBtn: '#btnCreateSupplier', modalId: '#supplierModal', formSelector: '#supplierForm', createUrl: '/Supplier/CreateSupplierModal', editBtn: '.btn-edit-supplier', editUrl: '/Supplier/EditSupplierModal?id=' },
        { createBtn: '#btnCreateCustomer', modalId: '#customerModal', formSelector: '#customerForm', createUrl: '/Customer/CreateCustomerModal', editBtn: '.btn-edit-customer', editUrl: '/Customer/EditCustomerModal?id=' },
        { createBtn: '#btnCreateMovement', modalId: '#movementModal', formSelector: '#movementForm', createUrl: '/InventoryMovement/CreateMovementModal', editBtn: '.btn-edit-movement', editUrl: '/InventoryMovement/EditMovementModal?id=' },
        { createBtn: '#btnCreatePurchase', modalId: '#purchaseModal', formSelector: '#purchaseForm', createUrl: '/Purchase/CreatePurchaseModal', editBtn: '.btn-edit-purchase', editUrl: '/Purchase/EditPurchaseModal?id=' },
    ];

    entities.forEach(e => {
        $(e.createBtn).click(function (ev) {
            ev.preventDefault();
            openEntityModal(e.modalId, e.createUrl);
        });

        $(document).on('click', e.editBtn, function (ev) {
            ev.preventDefault();
            var id = $(this).data('id');
            openEntityModal(e.modalId, e.editUrl + id);
        });

        handleFormSubmit(e.modalId, e.formSelector);
    });
});
