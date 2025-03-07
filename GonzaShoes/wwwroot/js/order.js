let pedido = [];
let selectedProduct = null;

function showModal(product) {
    selectedProduct = product;
    document.getElementById('modalProductName').innerText = product.Name;
    let colorSelect = document.getElementById('colorSelect');
    let sizeSelect = document.getElementById('sizeSelect');

    colorSelect.innerHTML = '';
    sizeSelect.innerHTML = '';

    product.Colors.forEach(color => {
        let option = document.createElement('option');
        option.value = color.ColorId;
        option.text = color.ColorName;
        colorSelect.appendChild(option);
    });

    // Seleccionar el primer color por defecto
    if (product.Colors.length > 0) {
        colorSelect.value = product.Colors[0].ColorId;
        actualizarTalles(product.Colors[0]);
    }

    colorSelect.addEventListener('change', function () {
        let selectedColorId = parseInt(colorSelect.value);
        let selectedColor = product.Colors.find(c => c.ColorId === selectedColorId);
        if (selectedColor) {
            actualizarTalles(selectedColor);
        }
    });

    // Restablecer la cantidad a 1
    document.getElementById('cantidad').value = 1;

    let modal = new bootstrap.Modal(document.getElementById('productModal'));
    modal.show();
}

function actualizarTalles(color) {
    let sizeSelect = document.getElementById('sizeSelect');
    sizeSelect.innerHTML = '';
    color.Sizes.forEach(size => {
        let option = document.createElement('option');
        option.value = size.SizeId;
        option.text = size.SizeNumber;
        sizeSelect.appendChild(option);
    });
}

function agregarProducto() {
    let colorId = parseInt(document.getElementById('colorSelect').value);
    let colorName = document.getElementById('colorSelect').selectedOptions[0].text;
    let sizeId = parseInt(document.getElementById('sizeSelect').value);
    let sizeName = document.getElementById('sizeSelect').selectedOptions[0].text;
    let cantidad = parseInt(document.getElementById('cantidad').value);

    let productoExistente = pedido.find(p => p.id === selectedProduct.Id && p.colorId === colorId && p.sizeId === sizeId);
    if (productoExistente) {
        productoExistente.cantidad += cantidad;
        productoExistente.total += selectedProduct.Price * cantidad;
    } else {
        pedido.push(
            {
                productId: selectedProduct.Id,
                name: selectedProduct.Name,
                price: selectedProduct.Price,
                cantidad: cantidad,
                total: selectedProduct.Price * cantidad,
                brandId: selectedProduct.BrandId,
                modelProductId: selectedProduct.ModelProductId,
                colorId: colorId,
                colorName: colorName,
                sizeId: sizeId,
                sizeName: sizeName
            });
    }
    actualizarPedido();
}

function eliminarProducto(id, colorId, sizeId) {
    pedido = pedido.filter(p => !(p.productId === id && p.colorId === colorId && p.sizeId === sizeId));
    actualizarPedido();
}

function actualizarPedido() {
    let tbody = document.getElementById("detalle-pedido-body");
    tbody.innerHTML = "";
    let subtotal = 0;
    pedido.forEach(p => {
        subtotal += p.total;
        tbody.innerHTML += `<tr><td>${p.cantidad}</td><td>${p.name} (${p.colorName}, ${p.sizeName})</td><td>$${p.price.toFixed(2)}</td><td>$${p.total.toFixed(2)}</td><td><button class="btn-remove" onclick="eliminarProducto(${p.productId}, ${p.colorId}, ${p.sizeId})">X</button></td></tr>`;
    });
    document.getElementById("subtotal").innerText = `$${subtotal.toFixed(2)}`;
    actualizarTotal(subtotal);
}

function actualizarTotal(subtotal) {
    let descuento = parseFloat(document.getElementById("descuento").value) || 0;
    let total = subtotal - descuento;
    document.getElementById("total").innerText = `$${total.toFixed(2)}`;
}

function filtrarProductos() {
    let filtro = document.getElementById("filtro-nombre").value.toLowerCase();
    let productos = document.querySelectorAll(".producto");
    productos.forEach(producto => {
        let nombre = producto.getAttribute("data-name").toLowerCase();
        if (nombre.includes(filtro)) {
            producto.style.display = "";  // Muestra la fila si coincide con el filtro
        } else {
            producto.style.display = "none";  // Oculta la fila si no coincide
        }
    });
}

function showPaymentModal() {
    let total = parseFloat(document.getElementById("total").innerText.replace('$', ''));
    document.getElementById("cash").value = total.toFixed(2);
    document.getElementById("debitCard").value = 0;
    document.getElementById("creditCard").value = 0;
    document.getElementById("transfer").value = 0;
    document.getElementById("remainingAmount").innerText = "$0.00";

    let modal = new bootstrap.Modal(document.getElementById('paymentModal'));
    modal.show();
}

function setPaymentMethod(method) {
    let total = parseFloat(document.getElementById("total").innerText.replace('$', ''));
    document.getElementById("cash").value = 0;
    document.getElementById("debitCard").value = 0;
    document.getElementById("creditCard").value = 0;
    document.getElementById("transfer").value = 0;
    document.getElementById(method).value = total.toFixed(2);
    calculateRemainingAmount();
}

function calculateRemainingAmount() {
    let total = parseFloat(document.getElementById("total").innerText.replace('$', ''));
    let cash = parseFloat(document.getElementById("cash").value) || 0;
    let debitCard = parseFloat(document.getElementById("debitCard").value) || 0;
    let creditCard = parseFloat(document.getElementById("creditCard").value) || 0;
    let transfer = parseFloat(document.getElementById("transfer").value) || 0;
    let remaining = total - (cash + debitCard + creditCard + transfer);

    if (remaining < 0) {
        alert("El monto total no puede exceder el total del pedido.");
        return;
    }

    document.getElementById("remainingAmount").innerText = `$${remaining.toFixed(2)}`;

    if (remaining === 0) {
        return;
    }

    if (cash > 0) {
        document.getElementById("cash").value = Math.max(0, cash + remaining).toFixed(2);
    } else if (debitCard > 0) {
        document.getElementById("debitCard").value = Math.max(0, debitCard + remaining).toFixed(2);
    } else if (creditCard > 0) {
        document.getElementById("creditCard").value = Math.max(0, creditCard + remaining).toFixed(2);
    } else if (transfer > 0) {
        document.getElementById("transfer").value = Math.max(0, transfer + remaining).toFixed(2);
    }
}

function updatePaymentAmounts() {
    let total = parseFloat(document.getElementById("total").innerText.replace('$', ''));
    let cash = parseFloat(document.getElementById("cash").value) || 0;
    let debitCard = parseFloat(document.getElementById("debitCard").value) || 0;
    let creditCard = parseFloat(document.getElementById("creditCard").value) || 0;
    let transfer = parseFloat(document.getElementById("transfer").value) || 0;
    let sum = cash + debitCard + creditCard + transfer;

    let remaining = total - sum;
    document.getElementById("remainingAmount").innerText = `$${remaining.toFixed(2)}`;

    if (remaining === 0) {
        return;
    }

    let maxValue = Math.max(cash, debitCard, creditCard, transfer);
    let maxField = null;

    if (cash === maxValue) {
        maxField = "cash";
    } else if (debitCard === maxValue) {
        maxField = "debitCard";
    } else if (creditCard === maxValue) {
        maxField = "creditCard";
    } else if (transfer === maxValue) {
        maxField = "transfer";
    }

    if (maxField) {
        document.getElementById(maxField).value = Math.max(0, parseFloat(document.getElementById(maxField).value) + remaining).toFixed(2);
    }

    calculateRemainingAmount();
}

function confirmarPago() {
    let orderItems = pedido.map(p => ({
        ProductId: p.productId,
        ProductName: p.name,
        BrandName: p.brandName,
        BrandId: p.brandId,
        modelProductId: p.modelProductId,
        ModelName: p.modelName,
        ColorId: p.colorId,
        ColorName: p.colorName,
        SizeId: p.sizeId,
        SizeNumber: p.sizeName,
        Quantity: p.cantidad,
        UnitPrice: p.price,
        Total: p.total,
        TotalWithNoDiscount: p.total
    }));

    let orderPayment = {
        Amount: parseFloat(document.getElementById("total").innerText.replace('$', '')),
        Cash: parseFloat(document.getElementById("cash").value) || 0,
        DebitCard: parseFloat(document.getElementById("debitCard").value) || 0,
        CreditCard: parseFloat(document.getElementById("creditCard").value) || 0,
        Transfer: parseFloat(document.getElementById("transfer").value) || 0
    };

    let discount = parseFloat(document.getElementById("descuento").value) || 0;

    let order = {
        OrderItems: orderItems,
        OrderPayment: orderPayment,
        Total: orderPayment.Amount,
        Discount: discount,
        TotalWithNoDiscount: orderPayment.Amount + discount
        //    Observation: document.getElementById("observation").value || ""
    };

    fetch('/Order/Save', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(order)
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                // Limpiar la pantalla de edición
                pedido = [];
                actualizarPedido();
                document.getElementById("descuento").value = 0;
                document.getElementById("total").innerText = "$0.00";
                document.getElementById("subtotal").innerText = "$0.00";
                $('#paymentModal').modal('hide');
            } else {
                // Mostrar errores de validación
                let errorContainer = document.getElementById("errorContainer");
                errorContainer.innerHTML = "";

                let errorElement = document.createElement("p");
                errorElement.className = "alert alert-danger";
                errorElement.innerText = data.errors;
                errorContainer.appendChild(errorElement);

                // Mostrar el contenedor de errores y ocultarlo después de 5 segundos
                errorContainer.style.display = "block";
                setTimeout(() => {
                    errorContainer.style.display = "none";
                }, 5000);

                // Cerrar el modal de formas de pago si hay errores
                $('#paymentModal').modal('hide');
            }
        })
        .catch(error => {
            console.error("Error:", error);
            alert("Error al guardar el pedido.");
            // Cerrar el modal de formas de pago si hay errores
            $('#paymentModal').modal('hide');
        });
}
