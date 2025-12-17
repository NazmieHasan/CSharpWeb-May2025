function AddStay(bookingId) {
    var container = $('#createStay');
    container.load('/Admin/StayManagement/Create?bookingId=' + bookingId, function () {
        if ($.validator && $.validator.unobtrusive) {
            $.validator.unobtrusive.parse(container);
        }
        container.toggle();
    });

    container.off('submit', 'form').on('submit', 'form', function (e) {
        e.preventDefault();
        var $form = $(this);

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
            });
    });
}