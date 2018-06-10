# EOSHackathon project
### iRespo

## Certificates verified on the EOS blockchain

#### Overview:
1. Three solutions:
  1. Sample social portal user profile page:
    - certificates listed with credentials
    - each certificate with possibility for verification request
   2. Certificates manager site
    - requests manager (verify or reject requests)
    - cerfiticates manager (add or remove certificate for certain user)
   3. Smart contracts
    - one for requests 
    - one for certificates
    - abi files with Ricardian Clauses filled in
2. User flow for Jane and John:
  1. Jane has social account with her certificates listed
  2. John clicks on verification request button next to certificate A
  - request is stored on the blockchain
  3. Jane receives request for certificate A verification on manager site
  4. Jane decides to verify certificate A for John
  5. Email is sent to John with certificate A verification credentials from the blockchain (a combined unique hash)
  - **next** hash verifcation to be done on the server side
3. User flow for University of London and Jane:
  1. University of London issues a cerificate B for Jane
  - there is a record in blockchain table with ceritficate ID and unique certificate hash for future verification
  2. Jane has new certificate she can list on her social page, for other users to verify!
  
    
