$(function () {
    $("#deleteForm").submit(function () {
        var certificateId = $("#CertificateId").data(certid);
        var privateKey = $("#AuthorEOSPrivateKey").val();

        eos = Eos({ keyProvider: eosauthorprivatekey, httpEndpoint: 'https://eoshackathon.eastasia.cloudapp.azure.com' })

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
        });
    })
});