import { useEffect, useState } from "react";
import { Guid } from "guid-typescript";
import { DelegatedAccessView } from "../../../models/views/components/delegatedAccess/delegatedAccessView";
import { delegatedAccessService } from "../../foundations/delegatedAccessService";

type DelegatedAccessViewServiceResponse = {
    mappedDelegatedAccesses: DelegatedAccessView[] | undefined;
    pages: Array<{ data: DelegatedAccessView[] }>;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: { pages: Array<{ data: DelegatedAccessView[] }> } | undefined;
    refetch: () => void;
};

export const delegatedAccessViewService = {
    useCreateDelegatedAccess: () => {
        return delegatedAccessService.useCreatedelegatedAccess();
    },

    useGetAllDelegatedAccesses: (searchTerm?: string): DelegatedAccessViewServiceResponse => {
        let query = `?$orderby=createdDate desc`;

        if (searchTerm) {
            query = query + `&$filter=contains(Name,'${searchTerm}')`;
        }

        const response = delegatedAccessService.useRetrieveAllDelegatedAccessPages(query);
        const [mappedDelegatedAccesses, setMappedDelegatedAccesses] = useState<Array<DelegatedAccessView>>();
        const [pages, setPages] = useState<Array<{ data: DelegatedAccessView[] }>>([]);

        useEffect(() => {
            if (response.data && response.data.pages) {
                const delegatedAccesses: Array<DelegatedAccessView> = [];
                response.data.pages.forEach((x: { data: DelegatedAccessView[] }) => {
                    x.data.forEach((delegatedAccess: DelegatedAccessView) => {
                        delegatedAccesses.push(new DelegatedAccessView(
                            delegatedAccess.id,
                            delegatedAccess.requesterEmail,
                            delegatedAccess.recipientEmail,
                            delegatedAccess.isDelegatedAccess,
                            delegatedAccess.isApproved,
                            delegatedAccess.data,
                            delegatedAccess.identifierColumn,
                            delegatedAccess.createdBy,
                            delegatedAccess.createdDate,
                            delegatedAccess.updatedBy,
                            delegatedAccess.updatedDate,
                        ));
                    });
                });

                setMappedDelegatedAccesses(delegatedAccesses);
                setPages(response.data.pages);
            }
        }, [response.data]);

        return {
            mappedDelegatedAccesses,
            pages,
            isLoading: response.isLoading,
            fetchNextPage: response.fetchNextPage,
            isFetchingNextPage: response.isFetchingNextPage,
            hasNextPage: !!response.hasNextPage,
            data: response.data,
            refetch: response.refetch
        };
    },

    useGetDelegatedAccessById: (id: Guid) => {
        const query = `?$filter=id eq ${id}`;
        const response = delegatedAccessService.useRetrieveAllDelegatedAccessPages(query);
        const [mappedDelegatedAccess, setMappedDelegatedAccess] = useState<DelegatedAccessView>();

        useEffect(() => {
            if (response.data && response.data.pages && response.data.pages[0].data[0]) {
                const delegatedAccess = response.data.pages[0].data[0];
                const delegatedAccessView = new DelegatedAccessView(
                    delegatedAccess.id,
                    delegatedAccess.requesterEmail,
                    delegatedAccess.recipientEmail,
                    delegatedAccess.isDelegatedAccess,
                    delegatedAccess.isApproved,
                    delegatedAccess.data,
                    delegatedAccess.identifierColumn,
                    delegatedAccess.createdBy,
                    delegatedAccess.createdDate,
                    delegatedAccess.updatedBy,
                    delegatedAccess.updatedDate
                );

                setMappedDelegatedAccess(delegatedAccessView);
            }
        }, [response.data]);

        return {
            mappedDelegatedAccess,
            ...response
        };
    },

    useUpdateDelegatedAccess: () => {
        return delegatedAccessService.useModifyDelegatedAccess();
    },

    useRemoveDelegatedAccess: () => {
        return delegatedAccessService.useRemoveDelegatedAccess();
    },
};