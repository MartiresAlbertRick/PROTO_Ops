jQuery.fn.putCursorAtEnd = function () {

    return this.each(function () {

        // Cache references
        var $el = $(this),
            el = this;

        // Only focus if input isn't already
        if (!$el.is(":focus")) {
            $el.focus();
        }

        // If this function exists... (IE 9+)
        if (el.setSelectionRange) {

            // Double the length because Opera is inconsistent about whether a carriage return is one character or two.
            var len = $el.val().length * 2;

            // Timeout seems to be required for Blink
            setTimeout(function () {
                el.setSelectionRange(len, len);
            }, 1);

        } else {

            // As a fallback, replace the contents with itself
            // Doesn't work in Chrome, but Chrome supports setSelectionRange
            $el.val($el.val());

        }

        // Scroll to the bottom, in case we're in a tall textarea
        // (Necessary for Firefox and Chrome)
        this.scrollTop = 999999;

    });

};

function validate($btn) {
    $btn.attr('disabled', 'disabled');
    $('input[type=text].input-required').each(function (index, item) {
        $(item).unbind('focusin');
        $(item).unbind('focusout');
        if ($(item).val() != '') {
            $(item).removeClass('unfilled');
            $(item).addClass('filled');
            allowSubmit();
        }
        $(item).on('focusin', function () {
            $(item).removeClass('filled');
            $(item).addClass('unfilled');
        });
        $(item).on('focusout', function () {
            if ($(item).val() == '') {
                $(item).removeClass('filled');
                $(item).addClass('unfilled');
            }
            else {
                $(item).removeClass('unfilled');
                $(item).addClass('filled');
            }
            allowSubmit();
        });
    });
    $('textarea.input-required').each(function (index, item) {
        $(item).unbind('focusin');
        $(item).unbind('focusout');
        if ($(item).val() != '') {
            $(item).removeClass('unfilled');
            $(item).addClass('filled');
            allowSubmit();
        }
        $(item).on('focusin', function () {
            $(item).removeClass('filled');
            $(item).addClass('unfilled');
        });
        $(item).on('focusout', function () {
            if ($(item).val() == '') {
                $(item).removeClass('filled');
                $(item).addClass('unfilled');
            }
            else {
                $(item).removeClass('unfilled');
                $(item).addClass('filled');
            }
            allowSubmit();
        });
    });
    $('select.input-required').each(function (index, item) {
        $(item).unbind('focusin');
        $(item).unbind('focusout');
        if ($(item).val() != 'def' && $(item).val() != '' && $(item).val() != null) {
            $(item).removeClass('unfilled');
            $(item).addClass('filled');
            allowSubmit();
        }
        $(item).on('focusin', function () {
            $(item).removeClass('filled');
            $(item).addClass('unfilled');
        });
        $(item).on('focusout', function () {
            if ($(item).val() != 'def' && $(item).val() != '' && $(item).val() != null) {
                $(item).removeClass('unfilled');
                $(item).addClass('filled');
            }
            else {
                $(item).removeClass('filled');
                $(item).addClass('unfilled');
            }
            allowSubmit();
        });
    });

    function allowSubmit() {
        $btn.removeAttr('disabled');
        $('.unfilled').each(function (index, item) {
            $btn.attr('disabled', 'disabled');
        });
    }
}