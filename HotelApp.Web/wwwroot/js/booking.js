function initBookingGuestSelectors() {
    document.querySelectorAll('.adults-select').forEach(adultsSelect => {
        const roomId = adultsSelect.id.replace('AdultsCount-', '');
        const maxGuests = parseInt(adultsSelect.dataset.maxguests);
        const childrenSelect = document.getElementById(`childCountSelect-${roomId}`);
        const babySelect = document.getElementById(`babyCountSelect-${roomId}`);

        function updateChildOptions() {
            const adults = parseInt(adultsSelect.value) || 0;
            childrenSelect.innerHTML = '<option value="0" selected>Please Select</option>';
            for (let i = 0; i <= maxGuests - adults; i++) {
                const option = document.createElement('option');
                option.value = i;
                option.text = i;
                childrenSelect.appendChild(option);
            }
        }

        function updateBabyOptions() {
            const adults = parseInt(adultsSelect.value) || 0;
            const children = parseInt(childrenSelect.value) || 0;
            const remainingCapacity = maxGuests - adults - children + 1;

            babySelect.innerHTML = '<option value="0" selected>Please Select</option>';
            for (let i = 1; i <= remainingCapacity; i++) {
                const option = document.createElement('option');
                option.value = i;
                option.text = i;
                babySelect.appendChild(option);
            }
        }

        adultsSelect.addEventListener('change', () => {
            updateChildOptions();
            updateBabyOptions();
        });

        childrenSelect.addEventListener('change', updateBabyOptions);

        updateChildOptions();
        updateBabyOptions();
    });
}

initBookingGuestSelectors();

// Use Session for booking
function saveRoomToSession(roomId) {
    const adults = document.getElementById(`AdultsCount-${roomId}`).value;
    const children = document.getElementById(`childCountSelect-${roomId}`).value;
    const babies = document.getElementById(`babyCountSelect-${roomId}`).value;
    const adultsError = document.getElementById(`adultsError-${roomId}`);

    if (!adults || adults === "0") {
        adultsError.textContent = "Please select number of adults";
        return false;
    }
    adultsError.textContent = "";

    fetch('/Booking/SaveRoomToSession', {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: `roomId=${roomId}&adults=${adults}&children=${children}&babies=${babies}`
    })
        .then(r => r.json())
        .then(data => {
            if (!data.success) {
                alert(data.message);
                return;
            }
            getRoomsInSession();
        });

    return false;
}

function removeRoomFromSession(roomId) {
    fetch('/Booking/RemoveRoomFromSession', {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: `roomId=${roomId}`
    })
        .then(r => r.json())
        .then(data => {
            getRoomsInSession();
        });
}

function getRoomsInSession() {
    fetch('/Booking/GetRoomsInSession')
        .then(r => r.json())
        .then(data => {
            const container = document.getElementById('sessionRoomContainer');
            container.innerHTML = '';

            let totalPrice = 0;

            const heading = document.createElement('p');
            heading.textContent = 'Booking selection';
            heading.className = 'fs-5'; 
            container.appendChild(heading);

            if (data.length === 0) {
                const emptySession = document.createElement('p');
                emptySession.textContent = 'Your selection is empty.';
                emptySession.className = 'mb-0'; 
                container.appendChild(emptySession);
                const totalEl = document.createElement('p');
                totalEl.textContent = `Grand Total: $${totalPrice.toFixed(2)}`;
                container.appendChild(totalEl);
                return;
            }

            const table = document.createElement('table');
            table.className = 'table table-bordered table-striped table-sm'; 

            const thead = document.createElement('thead');
            thead.innerHTML = `
                <tr>
                    <th>Category</th>
                    <th>Adults</th>
                    <th>Children</th>
                    <th>Babies</th>
                    <th>Price</th>
                    <th>Action</th>
                </tr>
            `;
            table.appendChild(thead);

            const tbody = document.createElement('tbody');

            data.forEach(room => {
                const roomTotal = Math.ceil(days) * room.categoryPrice;
                totalPrice += roomTotal;

                const tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${room.categoryName}</td>
                    <td>${room.adultsCount}</td>
                    <td>${room.childCount}</td>
                    <td>${room.babyCount}</td>
                    <td>${Math.ceil(days)} ${Math.ceil(days) === 1 ? 'day' : 'days'} x $${room.categoryPrice.toFixed(2)} = $${roomTotal.toFixed(2)}</td>
                    <td>
                        <button class="btn btn-outline-danger btn-sm" onclick="removeRoomFromSession('${room.roomId}')">
                            Remove
                        </button>
                    </td>
                `;
                tbody.appendChild(tr);
            });

            table.appendChild(tbody);
            container.appendChild(table);

            const actionDiv = document.createElement('div');
            actionDiv.className = 'd-flex justify-content-end align-items-center mt-2';

            const totalDiv = document.createElement('div');
            totalDiv.className = 'fw-bold me-3'; 
            totalDiv.textContent = `Grand Total: $${totalPrice.toFixed(2)}`;
            actionDiv.appendChild(totalDiv);

            const goToBooking = document.createElement('a');
            goToBooking.id = 'goToBookingBtn';
            goToBooking.className = 'btn btn-danger';
            goToBooking.href = `/Booking/Add?dateArrival=${dateArrival}&dateDeparture=${dateDeparture}`;
            goToBooking.textContent = 'Go to Booking';
            actionDiv.appendChild(goToBooking);

            container.appendChild(actionDiv);
        });
}

getRoomsInSession();

document.querySelectorAll('.show-next-room-btn').forEach(btn => {
    btn.addEventListener('click', function () {
        const categoryId = this.dataset.category;
        let nextIndex = parseInt(this.dataset.nextIndex);

        const nextRoom = document.querySelector(`.room-form-${categoryId}[data-room-index='${nextIndex}']`);
        if (nextRoom) {
            nextRoom.classList.remove('d-none');
            nextIndex++;
            this.dataset.nextIndex = nextIndex;

            if (!document.querySelector(`.room-form-${categoryId}[data-room-index='${nextIndex}']`)) {
                this.style.display = 'none';
            }
        }
    });
});