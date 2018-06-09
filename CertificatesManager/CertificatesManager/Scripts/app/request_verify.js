$(function () {
    
    $("#verifyForm").submit(function (e) {
        e.preventDefault();

        var ecc = eosjs_ecc;
        var certificateId = parseInt($("#certId").attr("data-certid"), 10);

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



                $("#verifyForm").unbind('submit').submit();
            });
        
    });

});