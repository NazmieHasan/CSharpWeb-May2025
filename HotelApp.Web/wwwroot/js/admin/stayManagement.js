function AddStay(bookingRoomId) {
    var container = $('#createStay-' + bookingRoomId);

    container.off('submit', 'form');

    container.load('/Admin/StayManagement/Create?bookingRoomId=' + bookingRoomId, function () {

        if ($.validator && $.validator.unobtrusive) {
            $.validator.unobtrusive.parse(container);
        }

        container.toggle();

        container.on('submit', 'form', function (e) {
            e.preventDefault();

            var $form = $(this);

            $form.find('button[type=submit]').prop('disabled', true);

            $.post($form.attr('action'), $form.serialize())
                .done(function (result) {
                    if (result.success) {
                        window.location.href = result.redirectUrl;
                        return;
                    }

                    container.html(result);

                    if ($.validator && $.validator.unobtrusive) {
                        $.validator.unobtrusive.parse(container);
                    }
                })
                .fail(function () {
                    alert('An error occurred while saving the stay. Please try again.');
                })
                .always(function () {
                    $form.find('button[type=submit]').prop('disabled', false);
                });
        });
    });
}