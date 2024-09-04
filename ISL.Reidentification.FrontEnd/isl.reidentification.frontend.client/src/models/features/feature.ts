import { FeatureDefinitions } from "../../featureDefinitions";

export class Feature {
    public feature: FeatureDefinitions;

    constructor(feature: keyof typeof FeatureDefinitions) {
        this.feature = FeatureDefinitions[feature];

        if (this.feature === undefined) {
            throw new Error("Unknown feature - ensure appSettings and the feature definitions match.");
        }
    }
}