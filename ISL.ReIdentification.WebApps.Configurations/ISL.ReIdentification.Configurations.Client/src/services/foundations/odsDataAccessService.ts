import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import OdsDataBroker from "../../brokers/apiBroker.odsData";
import { OdsData } from "../../models/odsData/odsData";

export const odsDataService = {
    useCreateOdsData: () => {
        const broker = new OdsDataBroker();
        const queryClient = useQueryClient();

        return useMutation((odsData: OdsData) => {
            return broker.PostOdsDataAsync(odsData);
        },
            {
                onSuccess: (variables: OdsData) => {
                    queryClient.invalidateQueries("OdsDataGetAll");
                    queryClient.invalidateQueries(["OdsDataGetById", { id: variables.id }]);
                }
            });
    },

    useRetrieveAllOdsData: (query: string) => {
        const broker = new OdsDataBroker();

        return useQuery(
            ["OdsDataGetAll", { query: query }],
            () => broker.GetAllOdsDataAsync(query),
            { staleTime: Infinity });
    },

    useRetrieveAllOdsDataPages: (query: string) => {
        const broker = new OdsDataBroker();

        return useInfiniteQuery(
            ["OdsDataGetAll", { query: query }],
            ({ pageParam }: { pageParam?: string }) => {
                if (!pageParam) {
                    return broker.GetOdsDataFirstPagesAsync(query)
                }
                return broker.GetOdsDataSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage: { nextPage?: string }) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useModifyOdsData: () => {
        const broker = new OdsDataBroker();
        const queryClient = useQueryClient();

        return useMutation((odsData: OdsData) => {
            return broker.PutOdsDataAsync(odsData);
        },
            {
                onSuccess: (data: OdsData) => {
                    queryClient.invalidateQueries("OdsDataGetAll");
                    queryClient.invalidateQueries(["OdsDataGetById", { id: data.id }]);
                }
            });
    },

    useRemoveOdsData: () => {
        const broker = new OdsDataBroker();
        const queryClient = useQueryClient();

        return useMutation((id: Guid) => {
            return broker.DeleteOdsDataByIdAsync(id);
        },
            {
                onSuccess: (data: { id: Guid }) => {
                    queryClient.invalidateQueries("OdsDataGetAll");
                    queryClient.invalidateQueries(["OdsDataGetById", { id: data.id }]);
                }
            });
    },
}