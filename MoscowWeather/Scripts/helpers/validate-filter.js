$(function () {
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
                max: 2020
            }
        },
        messages: {
            'month': {
                digits: "Только цифры",
                number: "Только цифры",
                min: "Номер месяца не может быть меньше 1 и больше 12",
                max: "Номер месяца не может быть меньше 1 и больше 12"
            },
            'year': {
                digits: "Только цифры",
                number: "Только цифры",
                min: "Номер года не может быть меньше 1950 и больше 2020",
                max: "Номер года не может быть меньше 1950 и больше 2020"
            
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