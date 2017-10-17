/**
 * Расширение к $.ics содержит вспомогательные методы для работы с URL'ами.
 */
; (function ($, undefined) {
    $.ics = $.extend($.ics, {
        urlHelper: {
            /**
             * Получить все параметры из url.
             * @param url Url из которого необходимо получить параметры.
             * @returns {object} Коллекция параметров ключ, значение.
             */
            getUrlParameters: function (url) {
                if (typeof url !== 'string') {
                    return {};
                }

                url = url.trim().replace(/^\?/, '');

                if (!url) {
                    return {};
                }

                return url.trim().split('&').reduce(function (ret, param) {
                    var parts = param.split('=');
                    ret[parts[0]] = parts[1] === undefined ? null : decodeURIComponent(parts[1]);
                    return ret;
                }, {});
            },

            /**
             * Получить значение параметра из url.
             * @param {string} url Url в котором находится параметр.
             * @param {string} parameterName Наименование параметра, который необходимо получить.
             * @returns {?string} Значение параметра.
             */
            getUrlParameter: function (url, parameterName) {
                if (typeof url !== 'string')
                    throw 'URL должен являться не пустой строкой.';

                var results = new RegExp('[\\?&]' + parameterName + '=([^&#]*)').exec(url);
                return results ? decodeURIComponent(results[1]) : undefined;
            },

            /**
             * Объединить URL с параметрами запроса.
             * @param {string} url Базовый URL, который уже может содержать параметры.
             * @param {...string} queryParams Один или несколько параметров запроса.
             * @example
             * combineUrl('http://example.com', {'arg1':'hello ', 'arg2':'world'}); // returns "http://example.com?arg1=hello%20&arg2=world"
             * combineUrl('http://example.com', 'arg1=hello ', 'arg2=world'); // returns "http://example.com?arg1=hello%20&arg2=world"
             * @returns {string} URL, объединенный с параметрами запроса.
             */
            combineUrl: function (url, queryParams) {
                if (typeof url !== 'string')
                    throw 'URL должен являться не пустой строкой.';

                var parameters;

                if (typeof queryParams === "object") {
                    parameters = Object.keys(queryParams).map(function (paramKey) {
                        return paramKey + '=' + queryParams[paramKey];
                    });
                }
                else {
                    parameters = arguments.slice(1);
                }

                for (var i = 0; i < parameters.length; i++) {
                    var joinChar = (i != 0 || url.indexOf('?') != -1) ? '&' : '?';
                    url += joinChar + encodeURI(parameters[i]);
                }

                return url;
            }
        }
    });
})(jQuery);