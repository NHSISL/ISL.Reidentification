import { useEffect, useState } from "react";
import { Guid } from "guid-typescript";
import { ImpersonationContextView } from "../../../models/views/components/impersonationContext/impersonationContextView";
import { impersonationContextService } from "../../foundations/impersonationContextService";

type ImpersonationContextViewServiceResponse = {
    mappedImpersonationContexts: ImpersonationContextView[] | undefined;
    pages: Array<{ data: ImpersonationContextView[] }>;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: { pages: Array<{ data: ImpersonationContextView[] }> } | undefined;
    refetch: () => void;
};

export const impersonationContextViewService = {
    useCreateImpersonationContext: () => {
        return impersonationContextService.useCreateimpersonationContext();
    },

    useGetAllImpersonationContexts: (searchTerm?: string): ImpersonationContextViewServiceResponse => {
        let query = `?$orderby=createdDate desc`;

        if (searchTerm) {
            query = query + `&$filter=contains(Name,'${searchTerm}')`;
        }

        const response = impersonationContextService.useRetrieveAllImpersonationContextPages(query);
        const [mappedImpersonationContexts, setMappedImpersonationContexts] = useState<Array<ImpersonationContextView>>();
        const [pages, setPages] = useState<Array<{ data: ImpersonationContextView[] }>>([]);

        useEffect(() => {
            if (response.data && response.data.pages) {
                const impersonationContexts: Array<ImpersonationContextView> = [];
                response.data.pages.forEach((x: { data: ImpersonationContextView[] }) => {
                    x.data.forEach((impersonationContext: ImpersonationContextView) => {
                        impersonationContexts.push(new ImpersonationContextView(
                            impersonationContext.id,
                            impersonationContext.requesterEmail,
                            impersonationContext.recipientEmail,
                            impersonationContext.isImpersonationContext,
                            impersonationContext.isApproved,
                            impersonationContext.data,
                            impersonationContext.identifierColumn,
                            impersonationContext.createdBy,
                            impersonationContext.createdDate,
                            impersonationContext.updatedBy,
                            impersonationContext.updatedDate,
                        ));
                    });
                });

                setMappedImpersonationContexts(impersonationContexts);
                setPages(response.data.pages);
            }
        }, [response.data]);

        return {
            mappedImpersonationContexts,
            pages,
            isLoading: response.isLoading,
            fetchNextPage: response.fetchNextPage,
            isFetchingNextPage: response.isFetchingNextPage,
            hasNextPage: !!response.hasNextPage,
            data: response.data,
            refetch: response.refetch
        };
    },

    useGetImpersonationContextById: (id: Guid) => {
        const query = `?$filter=id eq ${id}`;
        const response = impersonationContextService.useRetrieveAllImpersonationContextPages(query);
        const [mappedImpersonationContext, setMappedImpersonationContext] = useState<ImpersonationContextView>();

        useEffect(() => {
            if (response.data && response.data.pages && response.data.pages[0].data[0]) {
                const impersonationContext = response.data.pages[0].data[0];
                const impersonationContextView = new ImpersonationContextView(
                    impersonationContext.id,
                    impersonationContext.requesterEmail,
                    impersonationContext.recipientEmail,
                    impersonationContext.isImpersonationContext,
                    impersonationContext.isApproved,
                    impersonationContext.data,
                    impersonationContext.identifierColumn,
                    impersonationContext.createdBy,
                    impersonationContext.createdDate,
                    impersonationContext.updatedBy,
                    impersonationContext.updatedDate
                );

                setMappedImpersonationContext(impersonationContextView);
            }
        }, [response.data]);

        return {
            mappedImpersonationContext,
            ...response
        };
    },

    useUpdateImpersonationContext: () => {
        return impersonationContextService.useModifyImpersonationContext();
    },

    useRemoveImpersonationContext: () => {
        return impersonationContextService.useRemoveImpersonationContext();
    },
};