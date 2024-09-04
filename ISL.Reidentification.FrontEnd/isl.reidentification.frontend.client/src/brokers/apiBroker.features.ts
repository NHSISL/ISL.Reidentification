import { Feature } from "../models/features/feature";
import ApiBroker from "./apiBroker";

class FeatureBroker {
    relativeAuditUrl = '/api/features';

    private apiBroker: ApiBroker = new ApiBroker();

    async GetAllFeatureAsync(): Promise<Feature[]> {
        const url = this.relativeAuditUrl;

        const result = await this.apiBroker.GetAsync(url);
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        return result.data.map((feature: any) => new Feature(feature));
    }
}
export default FeatureBroker;