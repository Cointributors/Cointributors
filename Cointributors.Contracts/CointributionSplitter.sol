// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@openzeppelin/contracts/token/ERC20/IERC20.sol";

struct Recipient {
    address recipientAddress;
    uint256 portion;    // Portion of the donation (in basis points, 1% = 100)
    bool isSplitter;    // Whether the recipient is another CointributionSplitter contract
}

interface ICointributionSplitter {
    function donate(uint256 amount, address tokenAddress) external;
    function getRecipients() external view returns (Recipient[] memory);
}

contract CointributionSplitter {
    Recipient[] public recipients;

    event DonationReceived(address indexed donor, address indexed token, uint256 amount);

    constructor(Recipient[] memory _recipients) {
        uint256 totalPortion = 0;
        for (uint256 i = 0; i < _recipients.length; i++) {
            require(_recipients[i].portion > 0, "Portion must be greater than 0");
            totalPortion += _recipients[i].portion;
            recipients.push(_recipients[i]);
        }

        // Ensure the total portions add up to 100% (or 10,000 basis points)
        require(totalPortion == 10000, "Total portion must equal 10,000");
    }

    function donate(uint256 amount, address tokenAddress) public {
        require(amount > 0, "Amount must be greater than 0");
        IERC20 token = IERC20(tokenAddress);

        for (uint256 i = 0; i < recipients.length; i++) {
            uint256 recipientShare = (amount * recipients[i].portion) / 10000;

            if (recipients[i].isSplitter) {
                Recipient[] memory splitterRecipients = ICointributionSplitter(recipients[i].recipientAddress).getRecipients();

                for (uint256 j = 0; j < splitterRecipients.length; j++) {
                    uint256 splitterRecipientShare = (recipientShare * splitterRecipients[j].portion) / 10000;
                    require(token.transferFrom(msg.sender, splitterRecipients[j].recipientAddress, splitterRecipientShare), "Token transfer to splitter recipient failed");
                }
            } else {
                require(token.transferFrom(msg.sender, recipients[i].recipientAddress, recipientShare), "Token transfer failed");
            }
        }

        emit DonationReceived(msg.sender, tokenAddress, amount);
    }

    function getRecipients() external view returns (Recipient[] memory) {
        return recipients;
    }
}
