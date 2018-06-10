#include <eosiolib/eosio.hpp>

using namespace eosio;
using namespace std;

class certificates : public contract {
	using contract::contract;
public:
	certificates(action_name self) : contract(self)
		, _certificates(_self, _self) {}
	
	//@abi table 
	struct certificate {
		uint64_t certificateId;
		string certificateHash;
		name issuer;

		uint64_t primary_key() const { return certificateId; }

		EOSLIB_SERIALIZE(certificate, (certificateId)(certificateHash)(issuer))
	};

	multi_index<N(certificate), certificate> _certificates;

	//@abi action
	void addcert(uint64_t certificateId, string certificateHash, name issuer) {
		require_auth(issuer);

		auto iterator = _certificates.find(certificateId);
		eosio_assert(iterator == _certificates.end(), "Certificate with given Id already exists");

		_certificates.emplace(issuer, [&](auto& row) {
			row.certificateId = certificateId;
			row.certificateHash = certificateHash;
			row.issuer = issuer;
		});

		print("Certificate added");
	}

	//@abi action
	void delcert(uint64_t certificateId, name issuer) {
		require_auth(issuer);

		auto iterator = _certificates.find(certificateId);
		eosio_assert(iterator != _certificates.end(), "Certificate with given Id does not exist.");

		_certificates.erase(iterator);
		
		print("Certificate deleted");

	}
};

EOSIO_ABI(certificates, (addcert)(delcert))