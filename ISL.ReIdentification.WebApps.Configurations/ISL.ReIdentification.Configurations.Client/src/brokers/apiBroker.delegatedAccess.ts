import { AxiosResponse } from "axios";
import { Guid } from "guid-typescript";
import ApiBroker from "./apiBroker";
import { DelegatedAccess } from "../models/delegatedAccess/delegatedAccess";

class DelegatedAccessBroker {
    relativeDelegatedAccessUrl = '/api/delegatedAccess';
    relativeDelegatedAccessOdataUrl = '/odata/delegatedAccess'

    private apiBroker: ApiBroker = new ApiBroker();

    private processOdataResult = (result: AxiosResponse) => {
        const data = result.data.value.map((delegatedAccess: any) => new DelegatedAccess(delegatedAccess));

        const nextPage = result.data['@odata.nextLink'];
        return { data, nextPage }
    }

    async PostDelegatedAccessAsync(delegatedAccess: DelegatedAccess) {
        return await this.apiBroker.PostAsync(this.relativeDelegatedAccessUrl, delegatedAccess)
            .then(result => new DelegatedAccess(result.data));
    }

    async GetAllDelegatedAccessAsync(queryString: string) {
        const url = this.relativeDelegatedAccessUrl + queryString;

        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((delegatedAccess: any) => new DelegatedAccess(delegatedAccess)));
    }

    async GetDelegatedAccessFirstPagesAsync(query: string) {
        const url = this.relativeDelegatedAccessOdataUrl + query;
        return this.processOdataResult(await this.apiBroker.GetAsync(url));
    }

    async GetDelegatedAccessSubsequentPagesAsync(absoluteUri: string) {
        return this.processOdataResult(await this.apiBroker.GetAsyncAbsolute(absoluteUri));
    }

    async GetDelegatedAccessByIdAsync(id: Guid) {
        const url = `${this.relativeDelegatedAccessUrl}/${id}`;

        return await this.apiBroker.GetAsync(url)
            .then(result => new DelegatedAccess(result.data));
    }

    async PutDelegatedAccessAsync(delegatedAccess: DelegatedAccess) {
        return await this.apiBroker.PutAsync(this.relativeDelegatedAccessUrl, delegatedAccess)
            .then(result => new DelegatedAccess(result.data));
    }

    async DeleteDelegatedAccessByIdAsync(id: Guid) {
        const url = `${this.relativeDelegatedAccessUrl}/${id}`;

        return await this.apiBroker.DeleteAsync(url)
            .then(result => new DelegatedAccess(result.data));
    }
}
export default DelegatedAccessBroker;