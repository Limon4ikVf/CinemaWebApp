document.addEventListener("DOMContentLoaded", function () {
    const seats = document.querySelectorAll('.hall-grid .seat');

    seats.forEach(seat => {
        seat.addEventListener('click', function () {
            toggleSeat(this);
        });
    });
});
function toggleSeat(seat) {
    if (!seat.closest('.hall-grid')) {
        return;
    }

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

async function bookTickets() {
    const selectedSeats = document.querySelectorAll('.hall-grid .seat.selected');

    if (selectedSeats.length === 0) {
        alert("Виберіть місця для покупки!");
        return;
    }

    let seatsData = [];
    selectedSeats.forEach(s => {
        seatsData.push({
            row: parseInt(s.dataset.row),
            place: parseInt(s.dataset.place)
        });
    });
    const urlParams = new URLSearchParams(window.location.search);
    const sessionId = parseInt(urlParams.get('sessionId'));

    const bookingRequest = {
        sessionId: sessionId,
        seats: seatsData
    };

    try {
        const response = await fetch('/Ticket/BookTickets', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(bookingRequest)
        });

        if (response.ok) {
            alert("✅ Успіх! Квитки куплено.");
            window.location.reload();
        } else {
            const errorText = await response.text();
            alert("❌ Помилка: " + errorText);
        }
    } catch (error) {
        console.error('Error:', error);
        alert("Сталася помилка з'єднання з сервером.");
    }
}

