import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import PdsDataBroker from "../../brokers/apiBroker.pdsData";
import { PdsData } from "../../models/pds/pdsData";

export const pdsDataService = {
    useCreatePdsData: () => {
        const broker = new PdsDataBroker();
        const queryClient = useQueryClient();

        return useMutation((pdsData: PdsData) => {
            return broker.PostPdsDataAsync(pdsData);
        },
            {
                onSuccess: (variables: PdsData) => {
                    queryClient.invalidateQueries("PdsDataGetAll");
                    queryClient.invalidateQueries(["PdsDataGetById", { id: variables.id }]);
                }
            });
    },

    useRetrieveAllPdsData: (query: string) => {
        const broker = new PdsDataBroker();

        return useQuery(
            ["PdsDataGetAll", { query: query }],
            () => broker.GetAllPdsDataAsync(query),
            { staleTime: Infinity });
    },

    useRetrieveAllPdsDataPages: (query: string) => {
        const broker = new PdsDataBroker();

        return useInfiniteQuery(
            ["PdsDataGetAll", { query: query }],
            ({ pageParam }: { pageParam?: string }) => {
                if (!pageParam) {
                    return broker.GetPdsDataFirstPagesAsync(query)
                }
                return broker.GetPdsDataSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage: { nextPage?: string }) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useModifyPdsData: () => {
        const broker = new PdsDataBroker();
        const queryClient = useQueryClient();

        return useMutation((pdsData: PdsData) => {
            return broker.PutPdsDataAsync(pdsData);
        },
            {
                onSuccess: (data: PdsData) => {
                    queryClient.invalidateQueries("PdsDataGetAll");
                    queryClient.invalidateQueries(["PdsDataGetById", { id: data.id }]);
                }
            });
    },

    useRemovePdsData: () => {
        const broker = new PdsDataBroker();
        const queryClient = useQueryClient();

        return useMutation((id: Guid) => {
            return broker.DeletePdsDataByIdAsync(id);
        },
            {
                onSuccess: (data: { id: Guid }) => {
                    queryClient.invalidateQueries("PdsDataGetAll");
                    queryClient.invalidateQueries(["PdsDataGetById", { id: data.id }]);
                }
            });
    },
}