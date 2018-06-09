#include <eosiolib/eosio.hpp>

using namespace eosio;
using namespace std;

class certificates : public eosio::contract {
public:
	certificates(action_name self) : contract(self)
		, _certificates(_self, _self)
		, _requests {}
	
	//@abi table 
	struct certificate {
		uint64_t certificateId;
		string certificateHash;
		name issuer;

		uint64_t primary_key() const { return certificateId; }

		EOSLIB_SERIALIZE(certificate, (certificateId)(certificateHash)(issuer))
	};

	//@abi table 
	struct request{
		uint64_t requestId;
		name requestor;
		string email;
	};

	multi_index<N(certificate), certificate> _certificates;
	multi_index<N(request), request> _requests;

	//@abi action
	void addcertificate(uint64_t certificateId,	string certificateHash,	name issuer) {
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
	void delcertificate(uint64_t certificateId, name issuer) {
		require_auth(issuer);

		auto iterator = _certificates.find(certificateId);
		eosio_assert(iterator != _certificates.end(), "Certificate with given Id does not exist");

		_certificates.erase(iterator);
		
		print("Certificate deleted");

	}

	//@abi action
	void addrequest(uint64_t requestId,	name requestor,	string email) {
		require_auth(requestor);

		auto iterator = _requests.find(requestId);
		eosio_assert(iterator == _requests.end(), "Request with given Id already exists");

		_requests.emplace(requestor, [&](auto& row) {
			row.requestId = requestId;
			row.requestor = requestor;
			row.email = email;
		});

		print("Request added");
	}

	//@abi action
	void delrequest(uint64_t requestId, name requestor) {
		require_auth(requestor);

		auto iterator = _requests.find(requestId);
		eosio_assert(iterator != _requests.end(), "Request with given Id does not exist");

		_requests.erase(iterator);

		print("Request deleted");

	}
};

// EOSIO_ABI(certificates, (addcertificate)(delcertificate)(addrequest)(delrequest))