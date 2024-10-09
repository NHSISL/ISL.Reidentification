import ApiBroker from "./apiBroker";

class EntraUsersBroker {
    absoluteUrl = 'https://graph.microsoft.com/v1.0/users';
    scope = 'Directory.Read.All'
    
    private apiBroker: ApiBroker = new ApiBroker(this.scope);

    async FilterUsersAsync(emailAddressFragment: string) {
        const url = `${this.absoluteUrl}?$filter=startswith(mail,'${emailAddressFragment}')`;

        return await this.apiBroker.GetAsyncAbsolute(url)
            .then(result => result.data.value);
    }
}
export default EntraUsersBroker;