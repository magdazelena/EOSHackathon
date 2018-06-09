$(function () {
    var scatter;

    document.addEventListener('scatterLoaded', scatterExtension => {
        scatter = window.scatter;
        window.scatter = null;
    });

    $(".confirmButton").click(function () {
        var id = $(this).attr("id");


        const requiredFields = {
            personal: ['email'],
            accounts: [
                { blockchain: 'eos', host: 'eoshackathon.eastasia.cloudapp.azure.com', port: 8888 }
            ]
        };
        const network = {
            blockchain: 'eos',
            host: 'eoshackathon.eastasia.cloudapp.azure.com',
            port: 8888
        }


        scatter.getIdentity(requiredFields).then(identity => {
            var email = identity.personal.email;
            var eosname = identity.accounts[0].name;
            $.ajax({
                type: "POST",
                url: 'https://certificatesmanager.azurewebsites.net/Requests/Create',
                data: JSON.stringify({ "EOSRequestorName": eosname, "CertificateId": id, "Email" : email }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) { //call successfull
                    var requestId = data.requestId;
                    const eosOptions = {};
                    eos = Eos({ keyProvider: '5JSYsDkyCp3p7zsMcE2Sv7Ep3gii6Vm2wL4ED4dW5j2XQSFm38S', httpEndpoint: 'http://eoshackathon.eastasia.cloudapp.azure.com:8888' })
                    //const eos = scatter.eos(network, Eos.Localnet, eosOptions, 'http');
                    eos.transaction({
                        actions: [
                            {
                                account: 'requests',
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
                    //eos.contract('requests', {requiredFields}).then(requests => {
                    //    requests.transaction(r => {
                    //        r.addrequest(requestId, id, eosname, email)
                    //    })                        
                    //});
                    
                },
                error: function (xhr) {
                    //error occurred
                }
            });
        }).catch(error => {
            //...
        });
    });
});
