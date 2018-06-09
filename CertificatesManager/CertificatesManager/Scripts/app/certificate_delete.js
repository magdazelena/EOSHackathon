$(function () {
    $("#deleteForm").submit(function (e) {
        e.preventDefault();

        var form = $(this);
        var certificateId = $("#CertificateId").data("certid");
        var privateKey = $("#AuthorEOSPrivateKey").val();
        var eosauthor = $("#AuthorEos").data("authoreos");
        var submitForm = false;

        eos = Eos({ keyProvider: privateKey, httpEndpoint: 'https://eoshackathon.eastasia.cloudapp.azure.com' })

        eos.transaction({
            actions: [
                {
                    account: 'certificates',
                    name: 'delcert',
                    authorization: [{
                        actor: eosauthor,
                        permission: 'active'
                    }],
                    data: {
                        certificateId: certificateId,
                        issuer: eosauthor
                    }
                }
            ]
        }).then(function () {
            $("#deleteForm").unbind('submit').submit();

        });
       
    })
});