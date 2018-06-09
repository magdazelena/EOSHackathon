#include <eosiolib/eosio.hpp>

using namespace eosio;
using namespace std;

class certificates : public eosio::contract {
public:
	certificates(action_name self) : contract(self)
		, _certificate(_self, _self)
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
	void addcertificate(const account_name account, string& username) {
		/**
		* We require that only the owner of an account can use this action
		* or somebody with the account authorization
		*/
		require_auth(account);

		/**
		* We access the "player" table as creating an object of type "playerIndex"
		* As parameters we pass code & scope - _self from the parent contract
		*/
		playerIndex players(_self, _self);

		/**
		* We must verify that the account doesn't exist yet
		* If the account is not found the iterator variable should be players.end()
		*/
		auto iterator = players.find(account);
		eosio_assert(iterator == players.end(), "Address for account already exists");

		/**
		* We add the new player in the table
		* The first argument is the payer of the storage which will store the data
		*/
		players.emplace(account, [&](auto& player) {
			player.account_name = account;
			player.username = username;
			player.level = 1;
			player.health_points = 1000;
			player.energy_points = 1000;
		});
	}

};

// EOSIO_ABI(certificates, (addcertificate)(delcertificate)(addrequest)(delrequest))