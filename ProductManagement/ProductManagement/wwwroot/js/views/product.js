$(document).ready(function () {
    new Product();
});

class Product {
    constructor() {
        this.initEvents();
        this.loadData();
    }

    /**
     * Gán sự kiện cho các element
     * @param
     * Created by: TMSANG (21/06/2020)
     * */
    initEvents() {
        $('#insertOrUpdateModal').on('show.bs.modal', this.showInsertOrUpdateModal);
        $('#insertOrUpdateModal').on('shown.bs.modal', function () { $('#product-name').trigger('focus'); });
        $('#deleteModal').on('show.bs.modal', this.showDeleteModal);
        $(document).on('click', 'table tbody tr', this.tableRowOnClick);
        $(document).on('click', '.modal-footer .btn-save', this.btnSaveOnClick.bind(this));
        $(document).on('click', '.modal-footer .btn-confirm-delete', this.btnConfirmDeleteOnClick.bind(this));
        $(document).keypress(function (e) {
            if ($("#insertOrUpdateModal").hasClass('show') && (e.keycode == 13 || e.which == 13)) {
                $('.btn-save').trigger('focus');
            }
        });
    }

    /**
     * Hiển thị dialog thêm hoặc sửa
     * @param {any} event
     * Created by: TMSANG (21/06/2020)
     */
    showInsertOrUpdateModal(event) {
        var button = $(event.relatedTarget);
        var action = button.data("action");

        var modal = $(this);
        modal.find('.modal-title').text(action + ' a product')
        modal.attr('action', action);

        if (action == "Insert") {
            modal.find('.modal-body input').val('');
        }

        if (action == "Update") {
            var product = $('table tbody tr.table-primary');
            modal.find('#product-name').val(product.find('.product-name').text());
            modal.find('#product-price').val(product.find('.product-price').text().replace('$', ''));
        }
    }

    /**
     * Hiển thị dialog Xóa sản phẩm
     * @param {any} event
     * Created by: TMSANG (21/06/2020)
     */
    showDeleteModal(event) {
        var modal = $(this)
        var product = $('table tbody tr.table-primary');
        var productName = product.find('.product-name').text();
        modal.find('.modal-body .product-name b').text(productName);
    }

    /**
     * Xử lý sự kiện click 1 dòng trong bảng
     * @param {any} event
     * Created by: TMSANG (21/06/2020)
     */
    tableRowOnClick(event) {
        $('.action-button-group button[disabled]').removeAttr('disabled');
        var row = $(event.currentTarget);
        row.siblings().removeClass('table-primary');
        row.addClass('table-primary');
    }

    /**
     * Xử lý sự kiện click nút Lưu trong dialog Thêm mới hoặc Sửa sản phẩm
     * @param {any} event
     * Created by: TMSANG (21/06/2020)
     */
    btnSaveOnClick(event) {
        var action = $('#insertOrUpdateModal').attr('action');
        if (action == "Insert") {
            this.insertProduct();
        }
        if (action == "Update") {
            this.updateProduct();
        }
    }

    /**
     * Thêm mới sản phẩm
     * @param
     * Created by: TMSANG (21/06/2020)
     * */
    insertProduct() {
        var product = {
            Name: $('#product-name').val(),
            Price: parseFloat($('#product-price').val()),
            CreatedBy: 'admin',
            ModifiedBy: 'admin'
        };
        var productInstance = this;
        $.ajax({
            method: 'POST',
            url: '/api/products',
            data: JSON.stringify(product),
            contentType: 'application/json'
        }).done(function (res) {
            if (res.code == 200) {
                $('#insertOrUpdateModal').modal('hide');
                productInstance.loadData();
            }
        }).fail(function (res) {
        });
    }

    /**
     * Sửa thông tin sản phẩm
     * @param
     * Created by: TMSANG (21/06/2020)
     * */
    updateProduct() {
        var productId = parseInt($('table tbody tr.table-primary').find('.product-id').text());
        var product = {
            Id: productId,
            Name: $('#product-name').val(),
            Price: parseFloat($('#product-price').val()),
            CreatedBy: 'admin',
            ModifiedBy: 'admin',
            CreatedDate: $('table tbody tr.table-primary').data('createdDate')
        };
        var productInstance = this;
        $.ajax({
            method: 'PUT',
            url: `/api/products/${productId}`,
            data: JSON.stringify(product),
            contentType: 'application/json'
        }).done(function (res) {
            if (res.code == 200) {
                $('#insertOrUpdateModal').modal('hide');
                productInstance.loadData();
            }
        }).fail(function (res) {
        });
    }

    /**
     * Load dữ liệu vào bảng
     * @param
     * Created by: TMSANG (21/06/2020)
     * */
    loadData() {
        var productInstance = this;

        $.ajax({
            method: 'GET',
            url: '/api/products'
        }).done(function (res) {
            if (res.code == 200) {
                var products = res.data;
                var tableBody = $('table tbody');
                tableBody.empty();
                $.each(products, function (index, product) {
                    tableBody.append(`
                        <tr>
                            <td class="product-id" scope="row">${product.id}</td>
                            <td class="product-name">${product.name}</td>
                            <td class="product-price">$${product.price}</td>
                        </tr>
                    `);
                    tableBody.find('tr').last().data('createdDate', product.createdDate);
                });
            }

            productInstance.disableButtonByAction('Update');
            productInstance.disableButtonByAction('Delete');
        }).fail(function (res) {
        });
    }

    /**
     * Disable button theo action truyền vào
     * @param {any} action tên action (Insert/Update/Delete)
     * Created by: TMSANG (21/06/2020)
     */
    disableButtonByAction(action) {
        $(`button[data-action=${action}]`).attr('disabled', 'disabled');
    }

    /**
     * Xử lý sự kiện click nút Yes đồng ý xóa sản phẩm
     * @param {any} event
     * Created by: TMSANG (21/06/2020)
     */
    btnConfirmDeleteOnClick(event) {
        var productId = parseInt($('table tbody tr.table-primary').find('.product-id').text());
        var productInstance = this;
        $.ajax({
            method: 'DELETE',
            url: `/api/products/${productId}`
        }).done(function (res) {
            if (res.code == 200) {
                $('#deleteModal').modal('hide');
                productInstance.loadData();
            }
        }).fail(function (res) {
        });
    }
}
