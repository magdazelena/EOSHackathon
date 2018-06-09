$(function () {
    $('#createCertificateForm').submit(function () {

        $.ajax({
            type: $(this).attr("method"),
            url: $(this).attr("action"),
            data: $(this).serialize(),
            success: function (data) { //call successfull
                var certificateId = data.certificateId;
                alert(certificateId);
                /*

                eos = Eos({ keyProvider: '5JSYsDkyCp3p7zsMcE2Sv7Ep3gii6Vm2wL4ED4dW5j2XQSFm38S', httpEndpoint: 'http://eoshackathon.eastasia.cloudapp.azure.com:8888' })
              
                eos.transaction({
                    actions: [
                        {
                            account: 'requestsabcd',
                            name: 'addrequest',
                            authorization: [{
                                actor: 'johnjohnsons',
                                permission: 'active'
                            }],
                            data: {
                                requestId: requestId,
                                certificateId: id,
                                requestor: eosname,
                                email: email
                            }
                        }
                    ]
                });
                */
                // $('#infomodal').modal();
            },
            error: function (xhr) {
                //error occurred
            }
        });
        return false;
    });
});

