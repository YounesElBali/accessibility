import {NavItem, NavLink} from "reactstrap";
import { Link } from 'react-router-dom';

const uitloggen = () => {
    localStorage.removeItem("token");
    this.setState({
        toegang: localStorage.getItem("toegang"),
    });
    window.location.href = '/';
}

const ExpertNavMenu = () => {
    return (<>
        <NavItem>
            <NavLink tag={Link} className="text-light" to="/onderzoeken">
                Onderzoeken overzicht
            </NavLink> 
        </NavItem>
        <NavItem>
            <NavLink tag={Link} id='chatIndex' className="text-light" to="/chatIndex">
                Chat
            </NavLink>
        </NavItem>
        <NavItem>
            <NavLink tag={Link} id='signOut' className="text-light"  to="/" onClick={uitloggen} >
                Logout
            </NavLink>
        </NavItem>
    </>)
}

export default ExpertNavMenu;