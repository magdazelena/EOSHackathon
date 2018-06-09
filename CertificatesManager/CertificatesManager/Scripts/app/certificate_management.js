$(function () {
    $('#createCertificateForm').submit(function () {
        var author = $("#authorData").data("authorname");
        var eosauthor = $("#authorEosAccount").attr("data-eosauthor");

        $.ajax({
            type: $(this).attr("method"),
            url: $(this).attr("action"),
            data: $(this).serialize(),
            success: function (data) { //call successfull
                
                var ecc = eosjs_ecc;

                var certificateId = data.certificateId;
                var name = $("#Name").val(); 
                var placeofissue = $("#PlaceOfIssue").val();
                var content = $("#contentBox").val();
                var eosauthorprivatekey = $("#AuthorEOSPrivateKey").val();
                var eosowneraccount = $("#EOSOwnerAccount").val();

                var hash = ecc.sha256(name + "|" + author + "|" + placeofissue + "|" + content + "|" + eosowneraccount);

                eos = Eos({ keyProvider: eosauthorprivatekey, httpEndpoint: 'https://eoshackathon.eastasia.cloudapp.azure.com' })
              
                eos.transaction({
                    actions: [
                        {
                            account: 'certificates',
                            name: 'addcert',
                            authorization: [{
                                actor: eosauthor,
                                permission: 'active'
                            }],
                            data: {
                                certificateId: certificateId,
                                certificateHash: hash,
                                issuer: eosauthor
                            }
                        }
                    ]
                });
                
                 $('#infomodal').modal();
            },
            error: function (xhr) {
                //error occurred
            }
        });
        return false;
    });
});

