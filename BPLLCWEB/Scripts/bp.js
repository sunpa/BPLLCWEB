
$(document).ready(function () {

    $('#login-trigger').click(function () {
        $(this).next('#login-content').slideToggle();
        $(this).toggleClass('active');

        if ($(this).hasClass('active')) $(this).find('span').html('&#x25B2;')
        else $(this).find('span').html('&#x25BC;')
    })

    //$('a').on('click', function (e) {
    //    e.preventDefault();
    //});

    //$('li:has(ul) > a').on('click', function (e) {
    //    alert('hi');
    //    e.preventDefault();
    //});

    $('#ddmenu li').hover(function () {
        clearTimeout($.data(this, 'timer'));
        $('ul', this).stop(true, true).slideDown(200);
    }, function () {
        $.data(this, 'timer', setTimeout($.proxy(function () {
            $('ul', this).stop(true, true).slideUp(200);
        }, this), 100));
    });
});

 