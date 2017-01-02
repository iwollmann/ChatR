import React, { Component, PropTypes } from 'react'
import { uniq } from '../../services/array/array.js'

class RoomList extends Component {
    render() {
        function renderRooms(room) {
            return (
                <li key={room}>
                    {room}
                </li>);
        };

        return (
            <div className="roomList">
                <h3>Talking groups</h3>
                <ul id="rooms">
                    {uniq(this.props.rooms).map(renderRooms, this)}
                </ul>
                <a href="#" id="callCreateRoom" className="btns btnAddRoom icons icoAdd" alt="Criar sala">[ + ]</a>
            </div>
        )
    }
}

RoomList.propTypes = {
    rooms: PropTypes.arrayOf(PropTypes.string)
}

export default RoomList