// In localhost, patch Swagger UI's curl generator to always add -k for self-signed HTTPS
(function() {
    if (window.location.hostname === "localhost" || window.location.hostname === "127.0.0.1") {
        var interval = setInterval(function() {
            if (window.ui && window.ui.getConfigs && window.SwaggerUIBundle && window.SwaggerUIBundle.utils) {
                var oldCurl = window.SwaggerUIBundle.utils.buildCurl;
                if (oldCurl && !oldCurl._patchedWithK) {
                    window.SwaggerUIBundle.utils.buildCurl = function(request) {
                        var curl = oldCurl(request);
                        curl = curl.replace(/^curl /, "curl -k ");
                        return curl;
                    };
                    window.SwaggerUIBundle.utils.buildCurl._patchedWithK = true;
                }
                clearInterval(interval);
            }
        }, 500);
    }
})();
// This script runs on the Swagger UI page and automatically fills api-key if dev key is available
window.onload = function () {
    let keyName = "api-key";
    // Check the special variable set by backend (dev/local)
    let devApiKey = null;
    if (window.ui && window.ui.getConfigs) {
        var cfg = window.ui.getConfigs();
        if (cfg && cfg.DevApiKey) devApiKey = cfg.DevApiKey;
    }
    if (!devApiKey && window.DevApiKey) devApiKey = window.DevApiKey;

    if (devApiKey) {
        var intv = setInterval(() => {
            if (window.ui && window.ui.preauthorizeApiKey) {
                window.ui.preauthorizeApiKey(keyName, devApiKey);
                clearInterval(intv);
            }
        }, 500);
    }
};