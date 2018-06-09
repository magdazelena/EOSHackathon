$(function () {
    
    $("#verifyForm").submit(function (e) {
        e.preventDefault();

        var ecc = eosjs_ecc;
        var certificateId = parseInt($("#certId").attr("data-certid"), 10);
        var requestId = parseInt($("#requestId").attr("data-requestid"), 10);

        eos = Eos({ httpEndpoint: 'https://eoshackathon.eastasia.cloudapp.azure.com' });
        eos.getTableRows(
            {
                json: true,
                scope: "certificates",
                code: "certificates",
                table: "certificate"
            })
            .then(res => {
                var row = res.rows.filter(function (obj) {
                    return obj.certificateId == certificateId;
                });
                var hash = row[0].certificateHash;

                $.ajax({
                    type: "POST",
                    url: '/Requests/ProcessVerification',
                    data: JSON.stringify({ "CertificateHash": hash, "CertificateId": certificateId, "RequestId": requestId  }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $("#verifyForm").unbind('submit').submit();
                    }
                });
 
            });
        
    });

});