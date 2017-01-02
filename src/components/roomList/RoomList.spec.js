import React from 'react';
import { shallow } from 'enzyme';
import { expect } from 'chai';

import RoomList from './RoomList';

describe('<RoomList />', () => {
    it ('renders all rooms', ()=>{
        const rooms = ["room01", "room02", "room03"];
        const wrapper = shallow(<RoomList rooms = {rooms}/>);
        
        expect(wrapper.find('#rooms li').length).to.eq(3);
    });

    it ('not render duplicated rooms', ()=>{
        const rooms = ["room01", "room02", "room03", "room02"];
        const wrapper = shallow(<RoomList rooms = {rooms} />);

        expect(wrapper.find('#rooms li').length).to.eq(3);
    });
})