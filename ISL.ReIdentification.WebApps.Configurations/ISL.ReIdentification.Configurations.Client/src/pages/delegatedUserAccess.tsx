import { Container } from "react-bootstrap"
import BreadCrumbBase from "../components/bases/layouts/BreadCrumb/BreadCrumbBase"
import DelegatedUserAccessTable from "../components/delegatedUserAccess/delegatedUserAccessTable"

export const DelegatedUserAccess = () => {
    return (
        <Container fluid className="mt-4">
            <section>
                <BreadCrumbBase
                    link="/home"
                    backLink="Home"
                    currentLink="Delegated User Access">
                </BreadCrumbBase>
                <div className="mt-3">
                    <p>Role Needs to be in Security Matrix and Azure AD against user.</p>
                     <DelegatedUserAccessTable/>
                </div>
            </section>
        </Container>
    )
}