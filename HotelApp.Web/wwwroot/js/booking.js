function initBookingGuestSelectors(maxGuests) {
    const adultsSelect = document.getElementById('AdultsCount');
    const childrenSelect = document.getElementById('childCountSelect');
    const babySelect = document.getElementById('babyCountSelect');

    if (!adultsSelect || !childrenSelect || !babySelect) {
        return;
    }

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
}