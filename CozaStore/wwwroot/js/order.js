var connectionOrder = new signalR.HubConnectionBuilder().withUrl("/hubs/order").build();

connectionOrder.on("AddOrder", (data) => {
    toastr.success("New order #" + data.id);
    console.log(data);
    addNewRow(data);
})

connectionOrder.on("PaymentSuccess", (orderId) => {
    toastr.success("Order #" + orderId + " was paid");
    updatePaymentStatusSuccess(orderId);
})

function addNewRow(order) {
    let paymentStatusClass = order.paymentStatus === 1 ? "text-primary" : "text-secondary";
    let paymentStatusText = order.paymentStatus === 1 ? "<i class='fa-regular fa-check'></i> Paid"
        : "<i class='fa-sharp fa-regular fa-clock-three'></i> Pending";

    let orderStatusClass = order.orderStatus === 0 ? "bg-warning" :
        order.orderStatus === 1 ? "bg-primary" : "bg-danger";
    let orderStatusText = order.orderStatus === 0 ? "Unconfirmed" :
        order.orderStatus === 1 ? "Confirmed" : "Canceled";

    let newRow = `
    <tr data-order-id='${order.id}'>
            <td>${order.id}</td>
            <td>${order.name}</td>
            <td>${order.shipping}</td>
            <td>
                <span class="${paymentStatusClass}">
                    ${paymentStatusText}
                </span>
            </td>
            <td>${order.orderDate}</td>
            <td>
                <span class="badge ${orderStatusClass}">
                    ${orderStatusText}
                </span>
            </td>
            <td class="fw-bold">$${order.orderTotal.toFixed(2)}</td>
            <td>
                <div class="d-flex gap-2">
                    <a href="/admin/order/detail/?orderId=${order.id}" class="btn btn-sm btn-primary">
                        <i class="fa-sharp fa-solid fa-eye"></i>
                    </a>
                    <button type="button" onclick="Delete(this, '/admin/order/delete/?orderId=${order.id}');" class="btn btn-sm btn-danger">
                        <i class="fa-solid fa-trash-can"></i>
                    </button>
                </div>
            </td>
        </tr>
    `
    document.querySelector("table tbody").insertAdjacentHTML("afterbegin", newRow);
}

function updatePaymentStatusSuccess(orderId) {
    let row = document.querySelector(`tr[data-order-id='${orderId}']`);

    if (row) {
        let paymentCell = row.querySelector("td:nth-child(4) span");

        paymentCell.classList.remove("text-secondary");
        paymentCell.classList.add("text-primary");
        paymentCell.innerHTML = "<i class='fa-regular fa-check'></i> Paid";
    }
}

connectionOrder.start();