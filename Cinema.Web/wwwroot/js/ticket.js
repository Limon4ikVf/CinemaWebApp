function toggleSeat(seat) {
    if (seat.classList.contains('occupied')) return;

    seat.classList.toggle('selected');

    updatePanel();
}

function updatePanel() {
    const selectedSeats = document.querySelectorAll('.hall-grid .seat.selected');

    let totalCount = selectedSeats.length;
    let totalPrice = 0;

    selectedSeats.forEach(seat => {
        let priceString = seat.dataset.price;
        if (!priceString) return;

        priceString = priceString.toString().replace(',', '.').trim();
        const price = parseFloat(priceString);

        if (!isNaN(price)) {
            totalPrice += price;
        }
    });

    const countElement = document.getElementById('count');
    const totalElement = document.getElementById('total');
    const panel = document.getElementById('bookingPanel');

    if (countElement) countElement.innerText = totalCount;
    if (totalElement) totalElement.innerText = totalPrice.toFixed(0);


    if (totalCount > 0) {
        panel.classList.add('active');
    } else {
        panel.classList.remove('active');
    }
}

function bookTickets() {
    const selectedSeats = document.querySelectorAll('.hall-grid .seat.selected');

    if (selectedSeats.length === 0) {
        alert("Виберіть місця для покупки!");
        return;
    }

    let seatsData = [];
    selectedSeats.forEach(s => {
        seatsData.push(`Ряд ${s.dataset.row}, Місце ${s.dataset.place}`);
    });

    alert(`Ви обрали:\n${seatsData.join("\n")}\n\nЙдемо до оплати...`);
}