import 'https://code.jquery.com/jquery-1.7.1.min.js';
import './turn.min.js';

export function initTurn(elementId) {
    $('#' + elementId).turn({
        display: 'double',
        acceleration: true,
        gradients: true,
        elevation: 50
    });
}

export function nextPage(elementId) {
    $('#' + elementId).turn('next');
}

export function prevPage(elementId) {
    $('#' + elementId).turn('previous');
}
