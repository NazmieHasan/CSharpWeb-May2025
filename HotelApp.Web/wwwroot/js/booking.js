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

function appendGuestsToBooking(roomId) {
    const adults = document.getElementById(`AdultsCount-${roomId}`).value;
    const children = document.getElementById(`childCountSelect-${roomId}`).value;
    const babies = document.getElementById(`babyCountSelect-${roomId}`).value;
    const adultsError = document.getElementById(`adultsError-${roomId}`);

    if (!adults || adults === "0") {
        adultsError.textContent = "Please select number of adults";
        return false;
    } else {
        adultsError.textContent = "";
    }

    const bookingLink = event.currentTarget;
    const url = new URL(bookingLink.href);
    url.searchParams.set("AdultsCount", adults);
    url.searchParams.set("ChildCount", children || 0);
    url.searchParams.set("BabyCount", babies || 0);

    bookingLink.href = url.toString();
    return true;
}

initBookingGuestSelectors();