$(function () {
    console.log("валидация");
    $("#filter-date").validate({
        rules: {
            'month': {
                digits: true,
                number: true,
                min: 1,
                max: 12,
                dependence: '#year'
            },
            'year': {
                digits: true,
                number: true,
                min: 1950,
                max: 2020 //TODO: текущая дата?
            }
        },
        messages: {
            'month': {
                digits: "Только цифры",
                number: "Только цифры"
            },
            'year': {
                digits: "Только цифры",
                number: "Только цифры"
            }
        }
    })
});

$.validator.addMethod(
    "dependence",
    function (value, element, select) {
        var el = $(select);
        return this.optional(element) || el.val()!='';
    },
    "Введите год"
);